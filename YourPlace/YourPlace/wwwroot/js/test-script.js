document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('quizForm');
    const prevButton = document.querySelector('.prev-btn');
    const nextButton = document.querySelector('.next-btn');
    const questions = Array.from(form.querySelectorAll('.question'));
    let currentQuestion = 0;

    function showQuestion(index) {
        questions.forEach((question, i) => {
            question.style.display = i === index ? 'block' : 'none';
        });

        prevButton.style.display = index === 0 ? 'none' : 'inline-block';
        nextButton.textContent = index === questions.length - 1 ? 'Предай' : 'Следващ';
    }

    showQuestion(currentQuestion);

    prevButton.addEventListener('click', () => {
        if (currentQuestion > 0) {
            currentQuestion--;
            showQuestion(currentQuestion);
        }
    });

    nextButton.addEventListener('click', (e) => {
        e.preventDefault();

        const selectedAnswer = form.querySelector(`input[name="question${currentQuestion + 1}"]:checked`);

        if (selectedAnswer || currentQuestion === questions.length - 1) {
            if (currentQuestion < questions.length - 1) {
                currentQuestion++;
                showQuestion(currentQuestion);
            } else {
                console.log('hello');
                window.location.href = 'Sorting/ToSubmitPage';

            }
        } else {
            alert('Моля изберете отговор.');
        }
    });
});
