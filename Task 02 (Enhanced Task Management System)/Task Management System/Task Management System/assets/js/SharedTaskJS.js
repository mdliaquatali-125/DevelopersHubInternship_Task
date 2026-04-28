
document.addEventListener("DOMContentLoaded", function () {
    fetch("/api/tasks/GetAllSharedTasks")
        .then(res => res.json())
        .then(data => {
            let tableBody = document.getElementById("taskTableBody");
            tableBody.innerHTML = "";
            data.forEach(item => {
                let row = `
                    <tr>
                        <td>${item.Username}</td>
                        <td>${item.Title}</td>
                                        <td>${item.UploadFile ? `<a href="${item.UploadFile}" target="_blank">View File</a>` : 'No File'}</td>
                      <td>${new Date(item.SharedDate).toLocaleDateString('en-GB', {
                    day: '2-digit',
                    month: 'short',
                    year: 'numeric'
                })}</td>
                        <td>${item.CreatedBy}</td>
                        <td>${item.IsActive ? '<span class="badge badge-success">Active</span>' : '<span class="badge badge-danger">Inactive</span>'}</td>
                    </tr>
                `;
                tableBody.innerHTML += row;
            });
        })
        .catch(err => {
            Swal.fire({
                icon: "error",
                title: "Failed!",
                text: "API error occurred",
                showConfirmButton: true
            });
        });
});

function updateDateTime() {
    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes().toString().padStart(2, '0');
    const seconds = now.getSeconds().toString().padStart(2, '0');
    const day = now.toLocaleDateString('en-GB', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    // Greeting based on time
    let greeting = "Hello!";
    if (hours >= 5 && hours < 12) {
        greeting = "Good Morning!";
    } else if (hours >= 12 && hours < 17) {
        greeting = "Good Afternoon!";
    } else if (hours >= 17 && hours < 21) {
        greeting = "Good Evening!";
    } else {
        greeting = "Good Night!";
    }
    // Update DOM
    document.getElementById("greeting").innerText = greeting;
    document.getElementById("date-time").innerText = `${day} | ${hours}:${minutes}:${seconds}`;
}

// Update every second
setInterval(updateDateTime, 1000);
updateDateTime(); // Initial call