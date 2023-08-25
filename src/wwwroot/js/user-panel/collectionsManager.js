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

            handleResponse(response, "The Collection was created!", "Failed to create the Collection.");
        } else {
            alert("Failed to create the Collection.");
        }
    } catch (error) {
        console.error("An error occurred:", error);
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
            alert("Please enter a new title.");
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

            toggleElementVisibility(labelCollection, labelForTitle);
            toggleElementVisibility(selectedCollection, inputForTitle);

            handleResponse(response, "The Collection was renamed!", "Failed to rename the Collection.");
        } catch (error) {
            console.error("An error occurred:", error);
        }

        inputForTitle.removeEventListener("change", handleChange);
        renameButton.disabled = false; // Enable the button after the operation is complete
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
            handleResponse(response, "The Collection was deleted!", "Failed to delete the Collection.");
        } else {
            console.log("Failed to delete the Collection.");
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

async function handleResponse(response, successMessage, errorMessage) {
    if (!response.ok) {
        alert(errorMessage);
    } else {
        alert(successMessage);
    }
}

(async function () {
    await fetchAndPopulateCollections('rename-collection-select');
    await fetchAndPopulateCollections('delete-collection-select');
})();