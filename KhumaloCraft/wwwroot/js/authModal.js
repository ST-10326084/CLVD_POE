// Function to open the modal
function openAuthModal() {
    const modal = document.getElementById("authModal");
    modal.style.display = "block"; // Display the modal
}

// Function to close the modal
function closeAuthModal() {
    const modal = document.getElementById("authModal");
    modal.style.display = "none"; // Hide the modal
}

// Function to show the login section
function showLogin() {
    document.getElementById("loginSection").style.display = "block";
    document.getElementById("signUpSection").style.display = "none";
    document.getElementById("forgotPasswordSection").style.display = "none";
}

// Function to show the sign-up section
function showSignUp() {
    document.getElementById("loginSection").style.display = "none";
    document.getElementById("signUpSection").style.display = "block";
    document.getElementById("forgotPasswordSection").style.display = "none";
}

// Function to show the forgot password section
function showForgotPassword() {
    document.getElementById("loginSection").style.display = "none";
    document.getElementById("signUpSection").style.display = "none";
    document.getElementById("forgotPasswordSection").style.display = "block";
}

// Function to handle login form submission
function handleLogin(event) {
    event.preventDefault(); // Prevent default form submission

    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    // Perform login logic (e.g., AJAX request)
    console.log("Login with", email, password);

    closeAuthModal(); // Close the modal after handling
}

// Function to handle sign-up form submission
function handleSignUp(event) {
    event.preventDefault(); // Prevent default form submission

    const email = document.getElementById("signUpEmail").value;
    const password = document.getElementById("signUpPassword").value;

    // Perform sign-up logic (e.g., AJAX request)
    console.log("Sign up with", email, password);

    closeAuthModal(); // Close the modal after handling
}

// Function to handle forgot password form submission
function handleForgotPassword(event) {
    event.preventDefault(); // Prevent default form submission

    const email = document.getElementById("forgotPasswordEmail").value;

    // Perform password reset logic (e.g., AJAX request)
    console.log("Reset password for", email);

    closeAuthModal(); // Close the modal after handling
}
