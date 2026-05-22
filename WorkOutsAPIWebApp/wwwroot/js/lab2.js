const uriCategories = 'api/Categories';
const uriWorkouts = 'api/WorkOuts';
const uriExercises = 'api/Exercises';
const uriUsers = 'api/Users';

let categories = [];
let currentUser = null;
let currentWorkoutObj = null;
let lastActiveWorkoutPage = 'categories';
let currentWorkoutExercises = [];

if (!currentUser) {
    currentUser = {
        id: 1,
        nickname: "khrystyna",
        email: "k.fedorchuk@knu.ua",
        workOuts: [] 
    };
}

function navigateTo(section) {
    document.querySelectorAll('.app-page').forEach(p => p.style.display = 'none');
    document.querySelectorAll('nav button').forEach(b => b.classList.remove('active'));

    if (section === 'categories') {
        document.getElementById('categories-section').style.display = 'block';
        document.getElementById('nav-cat').classList.add('active');
        getCategories();
    } else if (section === 'profile') {
        document.getElementById('profile-section').style.display = 'block';
        document.getElementById('nav-prof').classList.add('active');
        getUserProfile();
    }
}

//категорії
function getCategories() {
    _displayAllEightCategories();
}

function _displayAllEightCategories() {
    const mockCategories = [
        { id: 1, name: "Meditation" },
        { id: 2, name: "Stretching" },
        { id: 3, name: "Cardio" },
        { id: 4, name: "HIIT" },
        { id: 5, name: "Yoga" },
        { id: 6, name: "Swimming" },
        { id: 7, name: "Pilates" },
        { id: 8, name: "Strength" }
    ];
    _displayCategories(mockCategories);
}

function _displayCategories(data) {
    const container = document.getElementById('categories');
    if (!container) return;

    container.innerHTML = '';
    categories = data;

    const counter = document.getElementById('counter');
    if (counter) counter.innerText = `Choose from ${data.length} categories`;

    data.forEach(category => {
        let displayTitle = category.name || category.categoryName;

        let card = document.createElement('div');
        card.className = 'category-row-card';
        card.innerHTML = `<span>${displayTitle}</span><span class="category-arrow">→</span>`;
        card.setAttribute('onclick', `openWorkoutsPage(${category.id}, '${displayTitle.replace(/'/g, "\\'")}')`);
        container.appendChild(card);
    });
}

// тренування
function openWorkoutsPage(categoryId, categoryName) {
    currentCategoryId = categoryId;
    currentCategoryName = categoryName;

    document.getElementById('workouts-title').innerText = `${categoryName} Workouts`;
    document.getElementById('categories-section').style.display = 'none';
    document.getElementById('workouts-section').style.display = 'block';

    getWorkouts();
}

function backToCategories() {
    document.getElementById('workouts-section').style.display = 'none';
    document.getElementById('categories-section').style.display = 'block';
}

function getWorkouts() {
    fetch(uriWorkouts)
        .then(response => response.json())
        .then(data => {
            let filtered = data.filter(w => w.categories && w.categories.some(c => c.id === currentCategoryId));
            if (filtered.length === 0) _generateMockWorkouts();
            else _displayWorkouts(filtered, 'workouts');
        })
        .catch(() => _generateMockWorkouts());
}

function _generateMockWorkouts() {
    let mock = [
        { id: 101, name: "Fat Burning HIIT", description: "High intensity • Fat burn", duration: 20, level: "Intermediate" },
        { id: 102, name: "Daily Core Activate", description: "Abs focus • No equipment", duration: 12, level: "Beginner" },
        { id: 103, name: "Deep Release Flow", description: "Cool down stretching", duration: 15, level: "All Levels" }
    ];
    _displayWorkouts(mock, 'workouts');
}

function _displayWorkouts(data, targetContainerId) {
    const container = document.getElementById(targetContainerId);
    if (!container) return;
    container.innerHTML = '';

    if (targetContainerId === 'workouts') {
        const counterEl = document.getElementById('workouts-counter');
        if (counterEl) counterEl.innerText = `${data.length} programs ready`;
    }

    data.forEach(workout => {
        let row = document.createElement('div');
        row.className = 'workout-row-card';

        let fromContext = (targetContainerId === 'user-workouts-list') ? 'profile' : 'categories';
        row.setAttribute('onclick', `openWorkoutDetailsPage(${JSON.stringify(workout)}, '${fromContext}')`);

        let detailsText = `${workout.description || 'Premium workout plan'} • ${workout.duration} min • ${workout.level || 'General'}`;

        row.innerHTML = `
            <div class="workout-img-placeholder"></div>
            <div class="workout-main-info">
                <h3>${workout.name}</h3>
                <p class="workout-sub-data">${detailsText}</p>
            </div>
        `;
        container.appendChild(row);
    });
}

//інфо тренування+вподобані
function openWorkoutDetailsPage(workout, fromContext) {
    currentWorkoutObj = workout;
    lastActiveWorkoutPage = fromContext;

    document.getElementById('selected-workout-name').innerText = workout.name;

    let detailsText = `${workout.description || 'Premium plan'} • ${workout.duration} min • ${workout.level || 'General'}`;
    document.getElementById('info-workout-subdata').innerText = detailsText;

    const isFav = currentUser && currentUser.workOuts && currentUser.workOuts.some(w => w.id === workout.id);
    const heartBtn = document.getElementById('fav-heart-btn');
    if (heartBtn) {
        if (isFav) {
            heartBtn.classList.add('is-fav');
        } else {
            heartBtn.classList.remove('is-fav');
        }
    }

    document.querySelectorAll('.app-page').forEach(p => p.style.display = 'none');
    document.getElementById('workout-details-section').style.display = 'block';

    getExercisesForWorkout(workout);
}

function toggleFavorite() {
    if (!currentUser) return;
    if (!currentUser.workOuts) currentUser.workOuts = [];

    const index = currentUser.workOuts.findIndex(w => w.id === currentWorkoutObj.id);
    const heartBtn = document.getElementById('fav-heart-btn');

    if (index > -1) {
        currentUser.workOuts.splice(index, 1);
        if (heartBtn) heartBtn.classList.remove('is-fav');
    } else {
        currentUser.workOuts.push(currentWorkoutObj);
        if (heartBtn) heartBtn.classList.add('is-fav');
    }

    const favCountEl = document.getElementById('fav-count');
    if (favCountEl) favCountEl.innerText = currentUser.workOuts.length;
}

function backToWorkouts() {
    document.getElementById('workout-details-section').style.display = 'none';

    if (lastActiveWorkoutPage === 'profile') {
        document.getElementById('profile-section').style.display = 'block';
        getUserProfile(); 
    } else {
        document.getElementById('workouts-section').style.display = 'block';
    }
}

// вправи
function getExercisesForWorkout(workout) {
    if (!workout.exercises || workout.exercises.length === 0) {
        fetch(uriExercises)
            .then(response => response.json())
            .then(data => {
                let filtered = data.filter(e => e.workOuts && e.workOuts.some(w => w.id === workout.id));
                if (filtered.length === 0) _generateMockExercises();
                else _displayExercises(filtered);
            })
            .catch(() => _generateMockExercises());
    } else {
        _displayExercises(workout.exercises);
    }
}

function _generateMockExercises() {
    let mockEx = [
        { id: 201, name: "Jumping Jacks", description: "Jump out with arms up, then jump back. Keep a steady fast pace throughout the set." },
        { id: 202, name: "Bodyweight Squats", description: "Lower your hips back like sitting on a chair. Keep heels on the floor and chest proud." },
        { id: 203, name: "Plank Hold", description: "Keep elbows under shoulders, body straight like a board. Tighten your core and don't drop hips." }
    ];
    _displayExercises(mockEx);
}

function _displayExercises(exercises) {
    currentWorkoutExercises = exercises;
    const container = document.getElementById('exercises-list');
    if (!container) return;
    container.innerHTML = '';

    exercises.forEach((ex, index) => {
        let row = document.createElement('div');
        row.className = 'exercise-row';
        row.setAttribute('onclick', `openExercisePage(${JSON.stringify(ex)})`);

        row.innerHTML = `
            <div class="exercise-left">
                <div class="exercise-num">${index + 1}</div>
                <strong style="font-size:16px;">${ex.name}</strong>
            </div>
            <span style="color:#a79288; font-weight:600;">➔</span>
        `;
        container.appendChild(row);
    });
}

function startFirstExercise() {
    if (currentWorkoutExercises.length > 0) {
        openExercisePage(currentWorkoutExercises[0]);
    }
}

function openExercisePage(exercise) {
    document.getElementById('exercise-title').innerText = exercise.name;
    document.getElementById('exercise-desc').innerText = exercise.description || 'Follow instructions carefully.';

    document.getElementById('workout-details-section').style.display = 'none';
    document.getElementById('exercise-page-section').style.display = 'block';
}

function backToWorkoutDetails() {
    document.getElementById('exercise-page-section').style.display = 'none';
    document.getElementById('workout-details-section').style.display = 'block';
}

// профіль
function getUserProfile() {
    fetch(uriUsers)
        .then(response => response.json())
        .then(usersList => {
            if (usersList && usersList.length > 0) {
                let serverUser = usersList[0];
                currentUser.nickname = serverUser.nickname;
                currentUser.email = serverUser.email;
            }
            _renderProfileDOM();
        })
        .catch(() => {
            _renderProfileDOM();
        });
}

function _renderProfileDOM() {
    document.getElementById('user-nickname').innerText = currentUser.nickname;
    document.getElementById('user-email').innerText = currentUser.email;

    const favCountEl = document.getElementById('fav-count');
    if (favCountEl) favCountEl.innerText = currentUser.workOuts.length;

    _displayWorkouts(currentUser.workOuts, 'user-workouts-list');
}