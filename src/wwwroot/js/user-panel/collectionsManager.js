const userId = document.getElementById('userId').value;

async function createCollection() {
    const title = document.getElementById('title').value.trim();

    if (!title) {
        alert("Please enter a title.");
        return;
    }

    try {
        const response = await fetch(`/api/collection?title=${title}&userId=${userId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        const collectionId = await response.text();
        if (response.ok) {
            const collectionSelectElements = document.querySelectorAll('#rename-collection-select, #delete-collection-select');

            collectionSelectElements.forEach(selectElement => {
                const option = document.createElement('option');
                option.value = collectionId;
                option.textContent = title;
                selectElement.appendChild(option);
            });

            // Clear the input field after successful creation
            document.getElementById('title').value = '';

            showNotification("Collection created!", 3000);
        } else if (response.status === 409) {
            showNotification("A collection with the same title already exists", 3000, true);
        }
        
    } catch (error) {
        console.error("An error occurred", error);
    }
}

async function renameCollection() {
    const renameButton = document.getElementById('rename-button');
    renameButton.disabled = true; // Disable the button to prevent multiple clicks

    const labelCollection = document.getElementById('base-collection-label');
    const selectedCollection = document.getElementById('rename-collection-select');
    const selectedCollectionId = selectedCollection.value;

    const labelForTitle = document.getElementById('rename-collection-label');
    const inputForTitle = document.getElementById('new-title');

    // Clear the input field 
    inputForTitle.value = '';

    toggleElementVisibility(labelForTitle, labelCollection);
    toggleElementVisibility(inputForTitle, selectedCollection);

    const handleChange = async () => {
        const newTitle = inputForTitle.value.trim();

        if (!newTitle) {
            showNotification("Please enter a new title.", 3000);
            return;
        }

        try {
            const response = await fetch(`/api/collection/?collectionId=${selectedCollectionId}&newTitle=${newTitle}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            const optionsToUpdate = document.querySelectorAll(`#rename-collection-select option[value='${selectedCollectionId}'], #delete-collection-select option[value='${selectedCollectionId}']`);
            optionsToUpdate.forEach(option => option.textContent = newTitle);

            showNotification("Collection name changed!", 3000);

            toggleElementVisibility(labelCollection, labelForTitle);
            toggleElementVisibility(selectedCollection, inputForTitle);


        } catch (error) {
            console.error("An error occurred:", error);
        }

        setTimeout(() => {
            renameButton.disabled = false;
        }, 500);

        inputForTitle.removeEventListener("change", handleChange);
    };

    inputForTitle.addEventListener("change", handleChange);
}

async function deleteCollection() {
    const selectedCollectionId = document.getElementById('delete-collection-select').value;

    try {
        const response = await fetch(`/api/collection/?collectionId=${selectedCollectionId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (response.ok) {
            removeOptionFromSelect('delete-collection-select', selectedCollectionId);
            removeOptionFromSelect('rename-collection-select', selectedCollectionId);

            showNotification("Collection deleted!", 3000);
        } else {
            showNotification("Failed to delete the Collection", 3000, true); 
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }
}

async function fetchAndPopulateCollections(selectElementId) {
    const response = await fetch(`/api/collections/?userId=${userId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    const data = await response.json();

    const collectionSelectElement = document.getElementById(selectElementId);

    data.forEach(collection => {
        const option = document.createElement('option');
        option.value = collection.id;
        option.textContent = collection.title;
        collectionSelectElement.appendChild(option);
    });
}

function removeOptionFromSelect(selectId, optionValue) {
    const selectElement = document.getElementById(selectId);
    const optionToRemove = selectElement.querySelector(`option[value='${optionValue}']`);
    if (optionToRemove) {
        optionToRemove.remove();
    }
}

function toggleContent(operation) {
    const selectedContent = document.getElementById(operation + '-content');
    if (selectedContent) {
        selectedContent.style.display = (selectedContent.style.display === 'block') ? 'none' : 'block';
    }
}

function toggleElementVisibility(showElement, hideElement) {
    showElement.style.display = "block";
    hideElement.style.display = "none";
}

function showNotification(message, duration, isError = false) {
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

(async function() {
    await fetchAndPopulateCollections('rename-collection-select');
    await fetchAndPopulateCollections('delete-collection-select');
})();