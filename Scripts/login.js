
const signupButton = document.getElementById('signUp')

const signInButton = document.getElementById('signIn')

const container = document.getElementById('container')

signupButton.addEventListener('click', () => {
    container.classList.add('right-panel-active')
})
signInButton.addEventListener('click', () => {
    container.classList.remove('right-panel-active')
})

function validatePass() {
    var pass = document.ggetElementById("Pass").value;
    var regex = /^(?=.*\d).{8,}$/;

    if (!regex.test(pass)) {
        alert("Password must have 1 number and at least 8 character!");
        return false;
    }
    return true;
}