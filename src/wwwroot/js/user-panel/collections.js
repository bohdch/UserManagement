const userId = document.getElementById('userId').value;

async function PickCollection() {
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

    document.getElementById('collections-details').style.display = 'block';
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

        if (!response.ok) {
            const data = await response.json();
            console.log("Error:", data.error);
        } else {
            console.log("Success: Book added to collection!");
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }
}