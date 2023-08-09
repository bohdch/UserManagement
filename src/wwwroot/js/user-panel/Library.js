// Constant
const CALLS_PER_LOAD = 3;

// Represents the current pages for API calls
let currentPageForAPI = {
    Popular: 1,
    Classic: 1
};

// Represents the pages requested from the database
let requestedPageFromDB = {
    Popular: 1,
    Classic: 1
};

let loadNextBooksCounter = {
    Popular: 0,
    Classic: 0
};


// Check if a page has been requested before
async function isPageRequested(category, page) {
    const response = await fetch(`/api/books/${category}/${page}/requested`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const result = await response.json();
        return result.requested;
    } else {
        console.error("Failed to check if page is requested");
        return false;
    }
}

// Fetch books for a specific category from API
async function fetchBooksFromAPI(category, page) {
    const pagesAvailable = await isPageRequested(category, page);
    if (pagesAvailable) {
        return null;
    }

    let apiUrl;
    if (category === 'Popular') {
        apiUrl = `https://gutendex.com/books/?sort=popular&page=${page}`;
    } else {
        apiUrl = `https://gutendex.com/books/?topic=${category}&page=${page}`;
    }

    const response = await fetch(apiUrl);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data.results;
}

// Save books data to the database
async function saveBooksToDatabase(books, category) {
    const response = await fetch('/api/books/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(books)
    });

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    if (category == "Popular" && (loadNextBooksCounter.Popular % CALLS_PER_LOAD != 0)) {
        await FetchPopularBooksFromDB();
    }

    if (category == "Classic" && (loadNextBooksCounter.Classic % CALLS_PER_LOAD != 0)) {
        await FetchBooksByCategoryFromDB('Classic');
    }
}

// Load the next page of books
async function loadNextPageOfBooks(category) {
    if (category === "Popular") {
        loadNextBooksCounter.Popular++;
        requestedPageFromDB.Popular++;

        await FetchPopularBooksFromDB();

        if (loadNextBooksCounter.Popular % CALLS_PER_LOAD === 0) {
            currentPageForAPI.Popular++;

            const books = await fetchBooksFromAPI(category, currentPageForAPI.Popular);
            await saveBooksToDatabase(books, category);
        }
    }

    if (category === "Classic") {
        loadNextBooksCounter.Classic++;
        requestedPageFromDB.Classic++;

        await FetchBooksByCategoryFromDB(category);

        if (loadNextBooksCounter.Classic % CALLS_PER_LOAD === 0) {
            currentPageForAPI.Classic++;

            const books = await fetchBooksFromAPI(category, currentPageForAPI.Classic);
            await saveBooksToDatabase(books, category);
        }
    }
}

// Fetch and display popular books
async function FetchBooksByCategoryFromDB(category) {
    const response = await fetch(`/api/books/${category}/${requestedPageFromDB.Classic}`, {
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

async function FetchPopularBooksFromDB() {
    const category = "Popular";

    const response = await fetch(`/api/books/popular/${requestedPageFromDB.Popular}`, {
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

// Event listeners for the "Load Next" buttons
document.getElementById('nextPopularButton').addEventListener('click', () => loadNextPageOfBooks('Popular'));
document.getElementById('nextClassicsButton').addEventListener('click', () => loadNextPageOfBooks('Classic'));


// Load and display popular books
async function loadPopularBooks() {
    const books = await fetchBooksFromAPI('Popular', currentPageForAPI.Popular);

    if (books !== null) {
        await saveBooksToDatabase(books, 'Popular');
    }
}

// Load and display classic books
async function loadClassicBooks() {
    const books = await fetchBooksFromAPI('Classic', currentPageForAPI.Classic);

    if (books !== null) {
        await saveBooksToDatabase(books, 'Classic');
    }
}

// Load initial set of books for both categories
async function loadInitialBooks() {
    await loadPopularBooks();
    await FetchPopularBooksFromDB();

    await loadClassicBooks();
    await FetchBooksByCategoryFromDB('Classic');
}

// Initial loading and display books
loadInitialBooks();
