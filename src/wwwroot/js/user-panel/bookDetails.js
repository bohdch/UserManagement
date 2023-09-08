let scrollPosition = 0;

const elements = {
    bookId: document.getElementById("book-id"),
    bookDetails: document.getElementById("book-details"),
    contentContainer: document.getElementById("content-container"),
    bookImage: document.getElementById("book-image"),
    bookTitle: document.getElementById("book-title"),
    bookAuthor: document.getElementById("book-author"),
    bookSubjects: document.getElementById("book-subjects"),
    bookBookshelves: document.getElementById("book-bookshelves"),
    bookLanguages: document.getElementById("book-languages"),
    bookDescription: document.getElementById("book-description"),
    bookLinkToContent: document.getElementById("book-content"),
};

async function redirectToBookDetails(bookData, authors) {
    const book = {
        id: bookData.id,
        image: bookData.formats['image/jpeg'],
        title: bookData.title,
        authors: authors,
        birth_year: bookData.authors.map(author => author.birth_year),
        death_year: bookData.authors.map(author => author.death_year),
        subjects: "Loading subjects...",
        bookshelves: "Loading bookshelves...",
        languages: "Loading languages...",
        description: "Loading description...",
        linkToContent: bookData.formats['text/html'],
    };

    scrollPosition = window.scrollY;
    displayBookDetails(book);

    const existingBook = await fetchExistingBookDetails(book.id);

    if (!existingBook || !existingBook.description) {
        const description = await fetchBookDescription(bookData.title, authors);

        Object.assign(book, {
            description,
            subjects: existingBook.subjects,
            bookshelves: existingBook.bookshelves,
            languages: existingBook.languages,
        });

        const bookDetailsViewModel = {
            Id: book.id,
            Description: description,
            Subjects: existingBook.subjects,
            Bookshelves: existingBook.bookshelves,
            languages: existingBook.languages,
        };

        await updateBookDetailsInDatabase(bookDetailsViewModel);
    } else {
        Object.assign(book, {
            description: existingBook.description,
            subjects: existingBook.subjects,
            bookshelves: existingBook.bookshelves,
            languages: existingBook.languages,
        });
    }

    updateBookDetails(book);
}

async function fetchBookDescription(title, author) {
    const response = await fetchBookInfo(title, author);
    const data = await response.json();

    const description =
        data?.items?.[0]?.volumeInfo?.description ||
        (await fetchAlternativeBookDescription(title, getSurname(author)));

    return description || 'Not available';
}

async function fetchAlternativeBookDescription(title, author) {
    const response = await fetchBookInfo(title, author);
    const data = await response.json();

    return data?.items?.[0]?.volumeInfo?.description || null;
}

async function fetchExistingBookDetails(bookId) {
    try {
        const response = await fetch(`/api/book-details/${bookId}`);
        if (response.ok) {
            return response.json();
        }
    } catch (error) {
        console.error("Failed to fetch existing book details", error);
    }
    return null;
}

async function updateBookDetailsInDatabase(book) {
    try {
        const response = await fetch('/api/book-details/update', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(book),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error("Failed to update book details in the database", error);
    }
}

function updateBookDetails(book) {
    elements.bookDescription.textContent = book.description;
    elements.bookSubjects.textContent = book.subjects;
    elements.bookBookshelves.textContent = book.bookshelves.length === 0 ? "Not available" : book.bookshelves;
    elements.bookLanguages.textContent = book.languages;
}

function displayBookDetails(book) {
    const { image, id, title, authors, birth_year, death_year, subjects, bookshelves, languages, description, linkToContent } = book;

    const authorNames = authors.split('; '); 
    const authorStrings = [];

    for (let i = 0; i < authorNames.length; i++) {
        const author = authorNames[i];
        const formattedBirthYear = formatYear(birth_year[i]);
        const formattedDeathYear = formatYear(death_year[i]);

        const authorString = `${author} [${formattedBirthYear} - ${formattedDeathYear}]`;
        authorStrings.push(authorString);
    }

    elements.bookImage.src = image;
    elements.bookImage.alt = "Book cover image";
    elements.bookId.textContent = id;
    elements.bookTitle.textContent = title;
    elements.bookAuthor.innerHTML = authorStrings.join(' and ');
    elements.bookSubjects.textContent = subjects;
    elements.bookBookshelves.textContent = bookshelves;
    elements.bookLanguages.textContent = languages;
    elements.bookDescription.textContent = description;
    elements.bookLinkToContent.href = linkToContent;

    openBookDetails();
}

async function fetchBookInfo(title, author) {
    const response = await fetch(`https://www.googleapis.com/books/v1/volumes?q=intitle:${title}+inauthor:${author}&maxResults=1&langRestrict=en&fields=items(volumeInfo(description))`);
    return response;
}

function getSurname(name) {
    return name.split(",")[0] || name.split(" ").pop();
}

function formatYear(year) {
    if (year && year < 0) {
        return `${Math.abs(year)} BCE`;
    } else if (year) {
        return year;
    } else {
        return 'N/A';
    }
}

function openBookDetails() {
    elements.bookDetails.style.display = "block";
    elements.contentContainer.style.display = "none";
}

function closeBookDetails() {
    elements.bookDetails.style.display = "none";
    elements.contentContainer.style.display = "block";
    window.scroll(0, scrollPosition);
}
