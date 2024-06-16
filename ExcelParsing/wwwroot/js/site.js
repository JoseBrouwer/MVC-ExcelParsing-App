function toggleInput(otherInputId, currentInput) {
    var otherInput = document.getElementById(otherInputId);
    if (currentInput.value.length > 0) {
        otherInput.disabled = true;
    } else {
        otherInput.disabled = false;
    }
}

function clearInput(otherInputId, inputId) {
    var otherInput = document.getElementById(otherInputId);
    var input = document.getElementById(inputId);
    input.value = '';
    otherInput.disabled = false;
}
