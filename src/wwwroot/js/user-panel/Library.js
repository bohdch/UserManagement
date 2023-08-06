// Constant
const ITEMS_PER_LOAD = 4; // Number of books to load per 'loadNextBooks' call

// State variables
let currentPageForAPI = 1; // Represents the current page for API calls in fetchPopularBooks
let requestedPageFromDB = 1; // Represents the page requested from the database in GetPopularBooks
let loadNextBooksCounter = 0;

// Check if pages are available in the local storage
function checkPagesInLocalStorage(page) {
    return localStorage.getItem(`page_${page}`) !== null;
}

// Save pages to the local storage
function savePagesInLocalStorage(page) {
    localStorage.setItem(`page_${page}`, "true");
}

// Fetch popular books from the API
async function fetchPopularBooks(page) {
    const pagesAvailable = checkPagesInLocalStorage(page);
    if (pagesAvailable) {
        return null;
    }

    const response = await fetch(`https://gutendex.com/books?sort=popular&page=${page}`);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();

    savePagesInLocalStorage(page);
    return data.results;
}

// Save books data to the database
async function saveBooksInDB(books) {
    const response = await fetch('/api/popular-books/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(books)
    });

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    await GetPopularBooks();
}

// Display books on the page
async function displayBooks(page) {
    const popularBookSet = document.querySelector('.book-heading[data-category="Popular"]').parentElement;
    const books = await fetchPopularBooks(page);

    if (books !== null) {
        await saveBooksInDB(books);
    }
}

// Load the next page of popular books
async function loadNextBooks() {
    requestedPageFromDB++;
    loadNextBooksCounter++;

    if (loadNextBooksCounter % ITEMS_PER_LOAD === 0) {
        currentPageForAPI++;
        displayBooks(currentPageForAPI);
    }

    await GetPopularBooks();
}

// Fetch and display popular books
async function GetPopularBooks() {
    const category = "Popular";

    const response = await fetch(`/api/popular-books/${requestedPageFromDB}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const books = await response.json();
        const categoryBookSet = document.querySelector(`.book-heading[data-category="${category}"]`).parentElement;

        books.forEach(bookData => {
            const bookItem = createBookElement(bookData);
            categoryBookSet.appendChild(bookItem);
        });

    } else {
        console.error("Failed to fetch data from the API");
    }
}

// Create a book element
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

    return bookItem;
}

// Event listener for the "Load Next" button
document.getElementById('nextButton').addEventListener('click', loadNextBooks);

// Initial loading and display of popular books
GetPopularBooks();
displayBooks(currentPageForAPI);
