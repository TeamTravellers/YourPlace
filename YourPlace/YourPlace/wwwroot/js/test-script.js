const question1 = document.getElementById('question1');
const hidden1 = document.getElementById('hidden-question1');

const question2 = document.getElementById('question2');
const hidden2 = document.getElementById('hidden-question2');

const question3 = document.getElementById('question3');
const hidden3 = document.getElementById('hidden-question3');

const question4 = document.getElementById('question4');
const hidden4 = document.getElementById('hidden-question4');

const question5 = document.getElementById('question5');
const hidden5 = document.getElementById('hidden-question5');

submitButton.addEventListener('click', (event) => {
    hidden1.value = getSelectedAnswer('question1');
    hidden2.value = getSelectedAnswer('question2');
    hidden3.value = getSelectedAnswer('question3');
    hidden4.value = getSelectedAnswer('question4');
    hidden5.value = getSelectedAnswer('question5');
});

function getSelectedAnswer(questionId) {
    const radioButtons = document.getElementById(questionId);
    let radioButtonValue = 0;
    for (const radioButton of radioButtons) {
        if (radioButton.checked) {
            radioButtonValue = radioButton.value;
        }
    }
    console.log("MEOWWWW");
    return radioButtonValue;
}