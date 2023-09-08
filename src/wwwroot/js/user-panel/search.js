var searchInput = document.getElementById('searchInput');
var searchIcon = document.getElementById('search-icon');
var searchTermPlaceholder = document.getElementById('searchTermPlaceholder');

const buttonsContainer = document.getElementById('Buttons');
const addToCollectionButton = document.getElementById('add-to-collection');

let page = 1;
let currentDisplayedBook = null;

async function fetchBooks(searchTerm, page) {
    const bookSet = document.getElementById('book-set');
    const loadingMessage = document.getElementById('message');

    // Set the content of the span element to the search term
    searchTermPlaceholder.textContent = searchTerm || "Unknown";

    // Display the loading message
    loadingMessage.style.display = 'block';
    loadingMessage.innerHTML = "Loading books...";

    // Clear any existing content
    bookSet.innerHTML = '';
    buttonsContainer.innerHTML = "";

    const response = await fetch(`https://gutendex.com/books/?search=${searchTerm}&page=${page}`, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        },
    });

    if (response.ok) {
        const data = await response.json();
        const books = data.results;
        books.forEach(bookData => bookSet.appendChild(createBookElement(bookData)));

        // Hide the loading message
        loadingMessage.style.display = 'none';

        buttonsContainer.innerHTML = "";

        if (data.previous !== null) {
            const previousButton = document.createElement('button');
            previousButton.textContent = "Previous";
            previousButton.addEventListener('click', async () => {
                page--;
                fetchBooks(searchTerm, page);
            });
            document.getElementById('Buttons').appendChild(previousButton);
        }

        if (data.next !== null) {
            const nextButton = document.createElement('button');
            nextButton.textContent = "Next";
            nextButton.addEventListener('click', async () => {
                page++;
                fetchBooks(searchTerm, page);
            });
            document.getElementById('Buttons').appendChild(nextButton);
        }

        // Check if no records were found
        if (books.length === 0) {
            loadingMessage.style.display = 'block';
            loadingMessage.innerHTML = "No records found";
        }
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
    const authors = bookData.authors && bookData.authors.length > 0 ? bookData.authors.reduce((acc, author) => acc + '; ' + author.name, '').slice(2) : 'Unknown';
    bookAuthor.textContent = authors;

    bookItem.append(bookImage, bookTitle, bookAuthor);

    bookItem.addEventListener('click', () => redirectToBookDetails(bookData, authors));

    return bookItem;
}

async function redirectToBookDetails(bookData, authors) {
    const book = {
        id: bookData.id,
        image: bookData.formats['image/jpeg'],
        title: bookData.title,
        authors: authors,
        birth_year: bookData.authors.map(author => author.birth_year),
        death_year: bookData.authors.map(author => author.death_year),
        subjects: bookData.subjects.length === 0 ? "Not available" : bookData.subjects.join(', '),
        bookshelves: bookData.bookshelves.length === 0 ? "Not available" : bookData.bookshelves.join(', '),
        languages: bookData.languages.join(', '),
        description: "Loading description...",
        linkToContent: bookData.formats['text/html'],
        bookAdded: false,
    };

    if (!book.linkToContent) {
        const textHtmlFormat = Object.keys(bookData.formats).find(key => key.includes('text/html'));
        book.linkToContent = textHtmlFormat ? bookData.formats[textHtmlFormat] : "Not available";
    }

    scrollPosition = window.scrollY;
    displayBookDetails(book);

    const description = await fetchBookDescription(bookData.title, authors);
    elements.bookDescription.textContent = description;
    book.description = description;

    currentDisplayedBook = book; 
}

async function PickCollection() {
    const collectionSelectElement = document.getElementById('collection-select');
    collectionSelectElement.innerHTML = ''; // Clear previous options

    const response = await fetch(`/api/collections/?userId=${userId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    const data = await response.json();

    data.forEach(collection => {
        const option = document.createElement('option');
        option.value = collection.id;
        option.textContent = collection.title;
        collectionSelectElement.appendChild(option);
    });

    document.getElementById('collections-details').style.display = 'block';

    collectionsDetails.classList.add('active');
    backdrop.classList.add('active');
}

async function addBook(bookData) {
    if (!bookData.bookAdded) {
        // Split the authors string into an array
        const authorNames = bookData.authors.split(';');
        const authorsList = [];

        for (let i = 0; i < authorNames.length; i++) {
            authorsList.push({
                name: authorNames[i],
                birth_year: bookData.birth_year[i],
                death_year: bookData.death_year[i],
            });
        }

        bookData.authors = authorsList;

        const formatsDictionary = {
            'text/html': bookData.linkToContent,
            'image/jpeg': bookData.image,
        };

        bookData.formats = formatsDictionary;

        await fetch('/api/book/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bookData),
        });

        // Set the bookAdded flag to true
        bookData.bookAdded = true;
    }

    currentDisplayedBook = bookData;
}

searchIcon.addEventListener('click', async function () {
    const newSearchTerm = searchInput.value.trim();
    if (newSearchTerm !== "") {
        fetchBooks(newSearchTerm);
        history.pushState({}, "", `?q=${newSearchTerm}`);
        window.location.reload();
    } else {
        showNotification("Please provide a book title or author's name", 3000, true);
    }
});

addToCollectionButton.addEventListener('click', async () => {
    if (currentDisplayedBook) {
        await addBook(currentDisplayedBook); 
    } 
});

// Initial fetch based on the initial query parameter
const urlParams = new URLSearchParams(window.location.search);
const initialSearchTerm = urlParams.get('q');
fetchBooks(initialSearchTerm, page);
