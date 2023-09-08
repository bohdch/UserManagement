const CALLS_PER_LOAD = 2;

const userId = document.getElementById('userId').value;
const collectionsDetails = document.getElementById('collections-details');
const backdrop = document.getElementById('backdrop');

const currentPageForAPI = {
    Popular: 1,
    Classic: 1,
    Fantasy: 1,
    Romance: 1,
    Mystery: 1
};

const requestedPageFromDB = {
    Popular: 1,
    Classic: 1,
    Fantasy: 1,
    Romance: 1,
    Mystery: 1
};

const loadNextBooksCounter = {
    Popular: 0,
    Classic: 0,
    Fantasy: 0,
    Romance: 0,
    Mystery: 0
};

async function fetchDataFromAPI(apiUrl) {
    const response = await fetch(apiUrl);

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json();
}

async function isPageRequested(category, page) {
    try {
        const response = await fetchDataFromAPI(`/api/books/${category}/${page}/requested`);
        return response.requested;
    } catch (error) {
        console.error("Failed to check if page is requested", error);
        return false;
    }
}

async function fetchBooksFromAPI(category, page) {
    const pagesAvailable = await isPageRequested(category, page);
    if (pagesAvailable) {
        return null;
    }

    const apiUrl = category === 'Popular'
        ? `https://gutendex.com/books/?sort=popular&page=${page}`
        : `https://gutendex.com/books/?topic=${category}&page=${page}`;

    return fetchDataFromAPI(apiUrl).then(data => data.results);
}

async function fetchAndSaveBooksFromAPI(category, page) {
    const books = await fetchBooksFromAPI(category, page);
    if (books !== null) {
        await saveBooksToDatabase(books, category);
    }
}

async function saveBooksToDatabase(books, category) {
    try {
        for (const book of books) {
            const { formats } = book;

            // Filter the formats to only keep "text/html" and "image/jpeg" formats
            const filteredFormats = {};
            if (formats["text/html"]) {
                filteredFormats["text/html"] = formats["text/html"];
            }
            if (formats["image/jpeg"]) {
                filteredFormats["image/jpeg"] = formats["image/jpeg"];
            }

            book.formats = filteredFormats;

            // Parsing arrays of strings into strings
            book.subjects = book.subjects.join(', ');
            book.bookshelves = book.bookshelves.join(', ');
            book.languages = book.languages.join(', ');
        }

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

        const counter = loadNextBooksCounter[category];
        if ((counter % CALLS_PER_LOAD !== 0) && (requestedPageFromDB[category] % CALLS_PER_LOAD !== 0)) {
            await fetchAndDisplayBooksFromDB(category);
        }
    } catch (error) {
        console.error("Failed to save books to the database", error);
    }
}

async function fetchAndDisplayBooksFromDB(category) {
    try {
        const response = await fetch(`/api/books/${category}/${requestedPageFromDB[category]}`, {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (response.ok) {
            const books = await response.json();
            const categoryBookSet = document.querySelector(`.book-heading[data-category="${category}"]`).nextElementSibling;
            books.forEach(bookData => categoryBookSet.appendChild(createBookElement(bookData)));
        } else {
            console.error("Failed to fetch data from the API");
        }
    } catch (error) {
        console.error("Failed to fetch and display books from the database", error);
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

async function loadNextPageOfBooks(category) {
    loadNextBooksCounter[category]++;
    requestedPageFromDB[category]++;

    await fetchAndDisplayBooksFromDB(category);

    if (loadNextBooksCounter[category] % CALLS_PER_LOAD === 0) {
        currentPageForAPI[category]++;

        await fetchAndSaveBooksFromAPI(category, currentPageForAPI[category]);
    }
}

async function loadInitialBooks() {
    const categories = ['Popular', 'Classic', 'Fantasy', 'Romance', 'Mystery'];

    for (const category of categories) {
        await fetchAndSaveBooksFromAPI(category, currentPageForAPI[category]);
        await fetchAndDisplayBooksFromDB(category);
    }
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

async function addBookToCollection() {
    const selectedCollectionId = document.getElementById('collection-select').value;
    const bookId = document.getElementById('book-id').textContent;

    const url = `/api/collection/add-book?bookId=${bookId}&collectionId=${selectedCollectionId}`;

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            showNotification("Book successfully added!", 3000);
            closeCollectionsDetails();
        }
        else if (response.status === 409) {
            showNotification("This book is already in the collection!", 3000, true);
            closeCollectionsDetails();
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }
}

function closeCollectionsDetails() {
    document.getElementById('collections-details').style.display = 'none';
    collectionsDetails.classList.remove('active');
    backdrop.classList.remove('active');
}

function showNotification(message, duration, isError) {
    const notification = document.getElementById('notification');

    notification.style.backgroundColor = '';
    notification.textContent = message;

    // Set background color to red for error notifications
    if (isError) {
        notification.style.backgroundColor = 'red';
    }

    notification.style.display = 'block';
    setTimeout(() => {
        notification.style.display = 'none';
    }, duration);
}

document.getElementById('nextPopularButton').addEventListener('click', () => loadNextPageOfBooks('Popular'));
document.getElementById('nextClassicsButton').addEventListener('click', () => loadNextPageOfBooks('Classic'));
document.getElementById('nextFantasyButton').addEventListener('click', () => loadNextPageOfBooks('Fantasy'));
document.getElementById('nextRomanceButton').addEventListener('click', () => loadNextPageOfBooks('Romance'));
document.getElementById('nextMysteryButton').addEventListener('click', () => loadNextPageOfBooks('Mystery'));

loadInitialBooks();