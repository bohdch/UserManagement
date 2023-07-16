async function getUsers() { 
    try {
         // Fetch data from the "/users" endpoint using GET method
        const response = await fetch("/api/users", {
            method: "GET",
            headers: {"Accept" : "application/json"}
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Parse the response as JSON
        const users = await response.json();
        const rows = document.querySelector("tbody");

        // clear existing rows
        rows.innerHTML = "";

        // If there are no users, display "no users" message
        if (users.length === 0) {
            const noUsersRow = document.createElement("tr");
            const noUsersCell = document.createElement("td");
            noUsersCell.setAttribute("colspan", "5");
            noUsersCell.setAttribute("id", "text");

            noUsersCell.textContent = "There are currently no users to display.";
            noUsersRow.appendChild(noUsersCell);
            rows.appendChild(noUsersRow);

        }
        else {
            // Iterate through the users and append a new row for each user
            users.forEach(user => {
                rows.appendChild(row(user));
            });
        }

    } catch (error) {
        console.error(error);
    }
}
async function deleteUser(id){ 
    try {
        // Fetch data from the "/users/{id}" endpoint using DELETE method
        const response = await fetch(`/api/users/${id}`, {
            method: "DELETE",
            headers: {"Accept" : "application/json"}
        })

        // If response is successful, remove the row for the deleted user from the table
        if (response.ok) {
            document.querySelector(`tr[data-rowid='${id}']`).remove();

            // If there are no users left, reload the page
            const rows = document.querySelectorAll("tbody tr");
            if (rows.length === 0) {
                location.reload();
            }

        } else {
            const error = await response.json();
            console.error(error);
        }
    } catch (error) {
        console.error(error);
    }
}

// Adding a new row to the table
function row (user) {
    
    // Set the data-rowid attribute to the user's id
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    // Create a table cell for the user's Name
    const nameTd = document.createElement("td");
    nameTd.append(user.name);
    tr.append(nameTd);

    // Create a table cell for the user's email
    const EmailTd = document.createElement("td");
    EmailTd.append(user.email);
    tr.append(EmailTd);

    // Create a table cell for the user's phone
    const phoneTd = document.createElement("td");
    phoneTd.append(user.phone || "-");
    tr.append(phoneTd);

    // Create a table cell for the user's CreatedAt
    const createdAtTd = document.createElement("td");
    const createdAtDate = new Date(user.createdAt);
    createdAtTd.append(createdAtDate.toLocaleString());
    tr.append(createdAtTd);

    // Create a table cell for the user's Verified status
    const verifiedTd = document.createElement("td");
    verifiedTd.append(user.verified ? "Yes" : "No");
    tr.append(verifiedTd);



    const linksTd = document.createElement("td");

    // Create a view button
    const updateLink = document.createElement("button"); 
    updateLink.append("View");
    linksTd.append(updateLink);
    
    // Event Handler for the 'Create' button
    updateLink.addEventListener("click", async() => await getUser(user.id));
    
    // Create a remove button
    const removeLink = document.createElement("button"); 
    removeLink.append("Remove");
    linksTd.append(removeLink);

    // Event Handler for the 'Remove' button
    removeLink.addEventListener("click", async () => await deleteUser(user.id));
    
    tr.appendChild(linksTd);
    return tr;
}
