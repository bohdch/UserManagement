const bookDetails = document.getElementById("book-details");
const contentContainer = document.getElementById("content-container");

const bookImageElement = document.getElementById("book-image");
const bookTitleElement = document.getElementById("book-title");
const bookAuthorElement = document.getElementById("book-author");
const bookDescriptionElement = document.getElementById("book-description");

const bookLinkToContentElement = document.getElementById("book-content");

let scrollPosition = 0;

async function redirectToBookDetails(bookData, authors) {
    const book = {
        id: bookData.id,
        image: bookData.formats['image/jpeg'],
        title: bookData.title,
        authors: authors,
        description: "Loading description...",
        linkToContent: bookData.formats['text/html']
    };

    scrollPosition = window.scrollY;
    displayBookDetails(book);


    const existingBook = await fetchExistingBookDetails(book.id);

    if (!existingBook || !existingBook.description) {
        const description = await fetchBookDescription(bookData.title, authors);
        book.description = description;

        const bookDetailsViewModel = {
            Id: book.id,
            Description: book.description
        };

        await updateBookDetailsInDatabase(bookDetailsViewModel);  
    } else {
        book.description = existingBook.description;
    }

    updateBookDetails(book);
}

async function fetchBookDescription(title, author) {
    const responsePromises = [
        fetch(`https://www.googleapis.com/books/v1/volumes?q=intitle:${title}+inauthor:${author}&maxResults=1&langRestrict=en&fields=items(volumeInfo(description))`),
        fetch(`https://www.googleapis.com/books/v1/volumes?q=intitle:${title}+inauthor:${getSurname(author)}&maxResults=1&langRestrict=en&fields=items(volumeInfo(description))`)
    ];

    const [response, alternativeResponse] = await Promise.allSettled(responsePromises);

    const [data, alternativeData] = await Promise.allSettled([response.value.json(), alternativeResponse.value.json()]);

    let description;

    description = data.value?.items?.[0]?.volumeInfo?.description || alternativeData.value?.items?.[0]?.volumeInfo?.description || 'Description not available';

    return description;
}

async function fetchExistingBookDetails(bookId) {
    try {
        const response = await fetch(`/api/book-details/${bookId}`);
        if (response.ok) {
            const data = await response.json();
            return data;
        }
    } catch (error) {
        console.error("Failed to fetch existing book details", error);
        return null;
    }
}

async function updateBookDetailsInDatabase(book) {
    try {
        const response = await fetch('/api/book-details/update', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(book)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

    } catch (error) {
        console.error("Failed to save books to the database", error);
    }
}

async function updateBookDetails(book) {
    bookDescriptionElement.textContent = book.description;
}

function getSurname(name) {
    return name.split(",")[0] || name.split(" ").pop();
}

function displayBookDetails(book) {
    bookImageElement.src = book.image;
    bookImageElement.alt = "Book cover image";

    bookTitleElement.textContent = book.title;
    bookAuthorElement.textContent = book.authors;
    bookDescriptionElement.textContent = book.description;
    bookLinkToContentElement.href = book.linkToContent;

    openBookDetails();
}

function openBookDetails() {
    bookDetails.style.display = "block";
    contentContainer.style.display = "none";
}

function closeBookDetails() {
    bookDetails.style.display = "none";
    contentContainer.style.display = "block";

    window.scroll(0, scrollPosition);
}
