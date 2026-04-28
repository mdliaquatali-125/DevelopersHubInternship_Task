
document.addEventListener("DOMContentLoaded", function () {

    let title = document.getElementById("Title");
    let desc = document.getElementById("Description");
    let status = document.getElementById("Status");
    let date = document.getElementById("DueDate");
    let file = document.getElementById("UploadFile");

    function onlyText(e) {
        e.target.value = e.target.value.replace(/[^A-Za-z\s.,]/g, "");
    }

    title.addEventListener("input", onlyText);
    desc.addEventListener("input", onlyText);

    // remove error
    title.addEventListener("input", () => title.classList.remove("error"));
    desc.addEventListener("input", () => desc.classList.remove("error"));
    status.addEventListener("change", () => status.classList.remove("error"));
    date.addEventListener("change", () => date.classList.remove("error"));
    file.addEventListener("change", () => file.classList.remove("error"));
});


function validateForm() {

    let isValid = true;

    let title = document.getElementById("Title");
    let desc = document.getElementById("Description");
    let status = document.getElementById("Status");
    let date = document.getElementById("DueDate");
    let file = document.getElementById("UploadFile");

    if (title.value.trim() === "") {
        title.classList.add("error");
        isValid = false;
    }

    if (desc.value.trim() === "") {
        desc.classList.add("error");
        isValid = false;
    }

    if (status.value === "Select Status") {
        status.classList.add("error");
        isValid = false;
    }

    if (date.value === "") {
        date.classList.add("error");
        isValid = false;
    }

    // ✅ FILE VALIDATION FIXED
    if (file.files.length === 0) {
        file.classList.add("error");
        isValid = false;
        return isValid;
    }

    let allowedExtensions = /(\.pdf|\.doc|\.docx|\.jpg|\.jpeg|\.png|\.zip)$/i;
    let selectedFile = file.files[0];

    if (!allowedExtensions.test(selectedFile.name)) {
        Swal.fire("Error", "Invalid file type", "error");
        file.value = "";
        isValid = false;
    }

    // ⚠️ better limit
    if (selectedFile.size > 50 * 1024 * 1024) {
        Swal.fire("Error", "File must be less than 50MB", "error");
        file.value = "";
        isValid = false;
    }

    return isValid;
}

document.getElementById("AddTask").addEventListener("submit", function (e) {

    e.preventDefault();

    if (!validateForm()) return;

    Swal.fire({
        icon: "info",
        title: "Please wait...",
        allowOutsideClick: false,
        didOpen: () => Swal.showLoading()
    });

    let fileInput = document.getElementById("UploadFile");
    let file = fileInput.files[0];

    let reader = new FileReader();

    reader.onload = function () {

        let data = {
            Title: document.getElementById("Title").value,
            Description: document.getElementById("Description").value,
            Status: document.getElementById("Status").value,
            DueDate: document.getElementById("DueDate").value,
            FileBase64: reader.result,
            FileName: file.name
        };

        fetch("/api/task/AddTask", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(res => {

                Swal.close();

                if (res.Status === "Success") {
                    Swal.fire("Success", res.Message, "success")
                        .then(() => window.location.href = res.URL);
                }
                else {
                    Swal.fire("Error", res.Message, "error");
                }
            })
            .catch(() => {
                Swal.fire("Error", "API failed", "error");
            });
    };

    reader.readAsDataURL(file);
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
