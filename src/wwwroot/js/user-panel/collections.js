const userId = document.getElementById('userId').value;
const collectionSelectElement = document.getElementById('collection-select');
const letterFilters = document.querySelectorAll('.letter-filter');

// Initialize collection select element
collectionSelectElement.innerHTML = '<option value="">Select a Collection</option>';

async function loadCollectionsAndBooks() {
    const response = await fetch(`/api/collections/?userId=${userId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    const data = await response.json();

    const collectionSelectElement = document.getElementById('collection-select');
    collectionSelectElement.innerHTML = '<option value="">Select a Collection</option>';

    data.forEach(collection => {
        const option = document.createElement('option');
        option.value = collection.id;
        option.textContent = collection.title;
        collectionSelectElement.appendChild(option);
    });

    // Attach an event listener to the select element
    collectionSelectElement.addEventListener('change', async (event) => {
        const selectedCollectionId = event.target.value;
        await fetchBooksFromCollection(selectedCollectionId);
    });
}

async function fetchBooksFromCollection(collectionId) {
    clearBooksAndLetterFilters();

    const response = await fetch(`/api/collection/books?collectionId=${collectionId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });

    if (response.ok) {
        const books = await response.json();
        const lettersWithBooks = new Set(books.map(book => book.title[0].toUpperCase()));

        updateLetterFilters(lettersWithBooks);

        books.forEach(book => {
            const bookElement = createBookElement(book);
            const firstLetter = book.title[0];

            const letterFilter = document.querySelector(`.letter-filter[letter="${firstLetter}"]`);
            const booksListElement = letterFilter.nextElementSibling;
            booksListElement.appendChild(bookElement);
        });
    } else {
        console.error('Failed to fetch books from the collection.');
    }
}

function createBookElement(bookData) {
    const bookItem = document.createElement('li');
    bookItem.className = 'book';

    const bookImage = document.createElement('img');
    bookImage.src = bookData.formats['image/jpeg'];
    bookImage.alt = 'Audio Book';

    const bookTitle = document.createElement('h3');
    bookTitle.textContent = bookData.title;

    const bookAuthor = document.createElement('p');
    const authors = bookData.authors && bookData.authors.length > 0 ? bookData.authors.map(author => author.name).join('; ') : 'Unknown';
    bookAuthor.textContent = authors;

    bookItem.append(bookImage, bookTitle, bookAuthor);

    bookItem.addEventListener('click', () => redirectToBookDetails(bookData, authors));

    return bookItem;
}

// Clear books and letter filters
function clearBooksAndLetterFilters() {
    letterFilters.forEach(letterFilter => {
        const booksListElement = letterFilter.nextElementSibling;
        booksListElement.innerHTML = '';
        letterFilter.style.display = 'none';
    });
}

// Update letter filters based on available books
function updateLetterFilters(lettersWithBooks) {
    letterFilters.forEach(letterFilter => {
        const letter = letterFilter.getAttribute('letter');
        letterFilter.style.display = lettersWithBooks.has(letter) ? 'block' : 'none';
    });
}

// Call the loadCollectionsAndBooks function when the page loads
document.addEventListener('DOMContentLoaded', async () => {
    await loadCollectionsAndBooks();
});