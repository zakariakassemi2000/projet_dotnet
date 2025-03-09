// wwwroot/js/calendar.js
document.addEventListener('DOMContentLoaded', function () {
    const today = new Date();
    let currentMonth = today.getMonth();
    let currentYear = today.getFullYear();

    const monthNames = ["janvier", "février", "mars", "avril", "mai", "juin",
        "juillet", "août", "septembre", "octobre", "novembre", "décembre"];

    let calendarTasks = [];

    function initializeTasks(tasks) {
        calendarTasks = tasks;
        renderCalendar(currentMonth, currentYear);
    }

    function renderCalendar(month, year) {
        const firstDay = new Date(year, month, 1);
        const lastDay = new Date(year, month + 1, 0);
        const daysInMonth = lastDay.getDate();

        // Mettre à jour le titre du mois
        document.getElementById('current-month').textContent = `${monthNames[month]} ${year}`;

        // Vider la grille du calendrier
        const calendarDays = document.getElementById('calendar-days');
        calendarDays.innerHTML = '';

        // Déterminer le premier jour de la semaine (0 = Dimanche, 1 = Lundi, ...)
        let startDay = firstDay.getDay();
        if (startDay === 0) startDay = 7; // Convertir Dimanche (0) à 7 pour commencer par Lundi
        startDay--; // Ajuster pour l'index 0

        // Ajouter les cellules vides pour les jours avant le premier jour du mois
        for (let i = 0; i < startDay; i++) {
            const emptyCell = document.createElement('div');
            emptyCell.className = 'day empty';
            calendarDays.appendChild(emptyCell);
        }

        // Ajouter les jours du mois
        for (let i = 1; i <= daysInMonth; i++) {
            const dayCell = document.createElement('div');
            dayCell.className = 'day';

            // Vérifier si c'est aujourd'hui
            if (i === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
                dayCell.classList.add('today');
            }

            // Ajouter le numéro du jour
            const dayNumber = document.createElement('div');
            dayNumber.className = 'day-number';
            dayNumber.textContent = i;
            dayCell.appendChild(dayNumber);

            // Ajouter les tâches pour ce jour
            calendarTasks.forEach(function (task) {
                const taskDate = new Date(task.dueDate);
                if (taskDate.getDate() === i && taskDate.getMonth() === month && taskDate.getFullYear() === year) {
                    const taskElement = document.createElement('div');
                    taskElement.className = 'task-event';
                    taskElement.setAttribute('data-task-id', task.id);

                    const taskTime = document.createElement('span');
                    taskTime.className = 'task-time';
                    taskTime.textContent = `${taskDate.getHours().toString().padStart(2, '0')}:${taskDate.getMinutes().toString().padStart(2, '0')}`;

                    taskElement.appendChild(taskTime);
                    taskElement.appendChild(document.createTextNode(task.title));

                    // Ajouter un événement de clic pour rediriger vers la page de détails
                    taskElement.addEventListener('click', function () {
                        window.location.href = `/Tasks/Details/${task.id}`;
                    });

                    dayCell.appendChild(taskElement);
                }
            });

            calendarDays.appendChild(dayCell);
        }
    }

    // Navigation dans le calendrier
    document.querySelector('.prev-month').addEventListener('click', function () {
        currentMonth--;
        if (currentMonth < 0) {
            currentMonth = 11;
            currentYear--;
        }
        renderCalendar(currentMonth, currentYear);
    });

    document.querySelector('.next-month').addEventListener('click', function () {
        currentMonth++;
        if (currentMonth > 11) {
            currentMonth = 0;
            currentYear++;
        }
        renderCalendar(currentMonth, currentYear);
    });

    // Exposer la fonction d'initialisation
    window.initializeTasks = initializeTasks;
});