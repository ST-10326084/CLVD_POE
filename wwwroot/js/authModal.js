function openAuthModal() {
    const modal = document.getElementById("authModal");
    modal.style.display = "block";
}

function closeAuthModal() {
    const modal = document.getElementById("authModal");
    modal.style.display = "none";
}

function showLogin() {
    document.getElementById("loginSection").style.display = "block";
    document.getElementById("signUpSection").style.display = "none";
    document.getElementById("forgotPasswordSection").style.display = "none";
}

function showSignUp() {
    document.getElementById("loginSection").style.display = "none";
    document.getElementById("signUpSection").style.display = "block";
    document.getElementById("forgotPasswordSection").style.display = "none";
}

function showForgotPassword() {
    document.getElementById("loginSection").style.display = "none";
    document.getElementById("signUpSection").style.display = "none";
    document.getElementById("forgotPasswordSection").style.display = "block";
}

function login() {
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    const data = {
        email: email,
        password: password
    };

    fetch('/User/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                alert('User logged in successfully.');
                closeAuthModal();
            } else {
                response.json().then(data => {
                    alert('Error logging in: ' + data.message);
                });
            }
        })
        .catch(error => {
            console.error('An error occurred:', error);
            alert('An error occurred. Please try again.');
        });
}

function signUp() {
    const email = document.getElementById("signUpEmail").value;
    const password = document.getElementById("signUpPassword").value;

    const data = {
        email: email,
        password: password
    };

    fetch('/User/SignUp', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                alert('User signed up successfully.');
                closeAuthModal();
            } else {
                response.json().then(data => {
                    alert('Error signing up: ' + data.message);
                });
            }
        })
        .catch(error => {
            console.error('An error occurred:', error);
            alert('An error occurred. Please try again.');
        });
}

function forgotPassword() {
    const email = document.getElementById("forgotPasswordEmail").value;

    const data = {
        email: email
    };

    fetch('/User/ForgotPassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                alert('Password reset instructions sent to your email.');
                closeAuthModal();
            } else {
                response.json().then(data => {
                    alert('Error resetting password: ' + data.message);
                });
            }
        })
        .catch(error => {
            console.error('An error occurred:', error);
            alert('An error occurred. Please try again.');
        });
}
