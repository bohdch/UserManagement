let currentPage = 1;

function createBookElement(bookData) {
    const bookItem = document.createElement('li');
    bookItem.className = 'book';

    const bookLink = document.createElement('a');
    bookLink.href = bookData.formats['text/html'];

    const bookImage = document.createElement('img');
    bookImage.src = bookData.formats['image/jpeg'];
    bookImage.alt = bookData.title;
    bookLink.appendChild(bookImage);
    bookItem.appendChild(bookLink);

    const bookTitle = document.createElement('h3');
    bookTitle.textContent = bookData.title;
    bookItem.appendChild(bookTitle);

    const bookAuthor = document.createElement('p');
    bookAuthor.textContent = bookData.authors[0].name;
    bookItem.appendChild(bookAuthor);

    bookItem.addEventListener('click', () => redirectToBookDescription(bookData));

    return bookItem;
}

async function fetchPopularBooks(page) {
    const response = await fetch(`https://gutendex.com/books?sort=popular&page=${page}`);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data.results;
}

async function displayBooks(page) {
    const popularBookSet = document.querySelector('.book-heading[data-category="Popular"]').parentElement;
    const books = await fetchPopularBooks(page);

    // Iterate through the results and create book elements
    books.forEach(bookData => {
        const bookItem = createBookElement(bookData);
        popularBookSet.appendChild(bookItem);
    });
}

// Function to load the next page of popular books
async function loadNextBooks() {
    currentPage += 1;
    await displayBooks(currentPage);
}

document.getElementById('nextButton').addEventListener('click', loadNextBooks);

// Initially load the first page of popular books
displayBooks(currentPage);
