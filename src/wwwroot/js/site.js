// Function to get all users
 async function getUsers() { 
    try {
         // Fetch data from the "/users" endpoint using GET method
        const response = await fetch("users", {
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
        else{
            // Iterate through the users and append a new row for each user
            users.forEach(user => {
                rows.appendChild(row(user));
            });
        }

    } catch (error) {
        console.error(error);
    }
}



// Function to get details of a single user
async function getUser(id){ 
    try {
        // Fetch data for the specified user ID from the "/users/{id}" endpoint using GET method
        const response = await fetch(`/users/${id}`, {
            method: "GET",
            headers : {"Accept" : "application/json"}
        });

        if(response.ok){
            // If response is successful, get the data as JSON
            const user = await response.json();

            // Assign the data from JSON to the respective fields in the form
            document.getElementById("userId").value = user.id;
            document.getElementById("FirstName").value = user.firstName;
            document.getElementById("LastName").value = user.lastName;
            document.getElementById("userAge").value = user.age;
            document.getElementById("Email").value = user.email;
        } else {
            // Handle error response
            console.error("Failed to get user details:", response.status);
        }
    } catch(error){
        // Handle any other errors
        console.error("Failed to get user details: ", error);
    }
}




// Function to create a new user
async function createUser() {
    const FirstNameInput = document.getElementById("FirstName");
    const LastNameInput = document.getElementById("LastName");
    const userAgeInput = document.getElementById("userAge");
    const EmailInput = document.getElementById("Email");

    // Check if all required fields are filled
    if (!FirstNameInput.value || !LastNameInput.value || !userAgeInput.value || !EmailInput.value) {
        alert("Please enter all required information.");
        return;
    }

    // Create an object with user data
    const userData = {
        firstName: FirstNameInput.value,
        lastName: LastNameInput.value,
        age: parseInt(userAgeInput.value),
        email: EmailInput.value
    };

    try {
        // Fetch data from the "/users" endpoint using POST method with user data in the request body
        const response = await fetch("/users", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(userData),
        });

        if (!response.ok) {
            throw new Error(`HTTP error ${response.status}`);
        }


        const newUser = await response.json();
        const newRow = row(newUser);

        document.querySelector("tbody").appendChild(newRow);

        // Reload the page to remove the "no users" text row when a new user is added
        const TextElement = document.getElementById("text");
        if (TextElement) {
            location.reload();
        }

    } catch (error) {
        console.error(error);
    }
} 

      

// Function to update a user
async function updateUser() {
    // Get input values from form
    const id = document.getElementById("userId").value;
    const firstName = document.getElementById("FirstName").value;
    const lastName = document.getElementById("LastName").value;
    const userAge = parseInt(document.getElementById("userAge").value);
    const email = document.getElementById("Email").value;

    // Create user data object
    const userData = {
        id : id,
        firstName: firstName,
        lastName: lastName,
        age: parseInt(userAge, 10),
        email : email
    };
    
    try {
        // Send PUT request to "/users" endpoint with user data in the request body
        const response = await fetch("/users", {
            method: "PUT",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(userData),
        })

        // If response is successful, replace the row for the updated user in the table
        if (response.ok === true) {
            const user = await response.json();
            document.querySelector(`tr[data-rowid='${user.id}']`).replaceWith(row(user));
        } else {
            // If response is unsuccessful, log the error message to console
            const error = await response.json();
            console.log(error.message);
        }
    } catch(error){
        console.error(error);
    }
}



// Function to delete a user
async function deleteUser(id){ 
    try {
        // Fetch data from the "/users/{id}" endpoint using DELETE method
        const response = await fetch(`/users/${id}`, {
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
            // If response is unsuccessful, log the error to console
            const error = await response.json();
            console.error(error);
        }
    } catch(error){
        // Log the error to console
        console.error(error);
    }
}




// Reset data after submission 
async function reset(){
    document.getElementById("userId").value = "";
    document.getElementById("FirstName").value = "";
    document.getElementById("LastName").value = "";
    document.getElementById("userAge").value = "";
    document.getElementById("Email").value = "";
}

// Adding a new row to the table
function row(user) {
    
    // Set the data-rowid attribute to the user's id
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    // Create a table cell for the user's First Name
    const FirsNameTd = document.createElement("td");
    FirsNameTd.append(user.firstName);
    tr.append(FirsNameTd);


    // Create a table cell for the user's Last Name
    const LastNameTd = document.createElement("td");
    LastNameTd.append(user.lastName);
    tr.append(LastNameTd);

    // Create a table cell for the user's age
    const ageId = document.createElement("td");
    ageId.append(user.age);
    tr.append(ageId);

    // Create a table cell for the user's email
    const EmailTd = document.createElement("td");
    EmailTd.append(user.email);
    tr.append(EmailTd);

    



    const linksTd = document.createElement("td");

    // Create an update button
    const updateLink = document.createElement("button"); 
    updateLink.append("Update");
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
