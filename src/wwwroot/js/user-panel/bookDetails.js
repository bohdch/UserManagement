const bookDetails = document.getElementById("book-details");
const contentContainer = document.getElementById("content-container");

const bookImageElement = document.getElementById("book-image");
const bookTitleElement = document.getElementById("book-title");
const bookAuthorElement = document.getElementById("book-author");
const bookDescriptionElement = document.getElementById("book-description");
const bookLinkToContentElement = document.getElementById("book-content");

let scrollPosition = 0;

async function fetchBookDescription(title, author) {
    const responsePromises = [
        fetch(`https://www.googleapis.com/books/v1/volumes?q=intitle:${title}+inauthor:${author}&maxResults=1&langRestrict=en&fields=items(volumeInfo(description))`),
        fetch(`https://www.googleapis.com/books/v1/volumes?q=intitle:${title}&maxResults=1&langRestrict=en&fields=items(volumeInfo(description))`)
    ];

    const [response, alternativeResponse] = await Promise.allSettled(responsePromises);

    const [data, alternativeData] = await Promise.allSettled([response.value.json(), alternativeResponse.value.json()]);

    let description;

    description = data.value?.items?.[0]?.volumeInfo?.description || alternativeData.value?.items?.[0]?.volumeInfo?.description || 'Description not available';

    return description;
}

async function redirectToBookDetails(bookData, authors) {
    const book = {
        image: bookData.formats['image/jpeg'],
        title: bookData.title,
        authors: authors,
        description: "Loading description...",
        linkToContent: bookData.formats['text/html']
    };

    scrollPosition = window.scrollY;
    displayBookDetails(book);

    fetchBookDescription(bookData.title, authors)
        .then(description => {
            book.description = description;
            updateBookDetails(book);
        });
}

function updateBookDetails(book) {
    bookDescriptionElement.textContent = book.description;
    bookLinkToContentElement.href = book.linkToContent;
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
