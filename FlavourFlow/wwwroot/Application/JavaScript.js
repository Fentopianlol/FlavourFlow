// Filter tabs functionality
const filterTabs = document.querySelectorAll('.filter-tab');
filterTabs.forEach(tab => {
    tab.addEventListener('click', () => {
        filterTabs.forEach(t => t.classList.remove('active'));
        tab.classList.add('active');
    });
});

// Cuisine card hover effects
const cuisineCards = document.querySelectorAll('.cuisine-card');
cuisineCards.forEach(card => {
    card.addEventListener('click', () => {
        const cuisineType = card.querySelector('h3').textContent;
        alert(`Exploring ${cuisineType} recipes...`);
    });
});

// Search functionality
const searchBtn = document.querySelector('.search-btn');
const searchInput = document.querySelector('.search-input');

searchBtn.addEventListener('click', () => {
    if (searchInput.value.trim()) {
        alert(`Searching for: ${searchInput.value}`);
    }
});

searchInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter' && searchInput.value.trim()) {
        alert(`Searching for: ${searchInput.value}`);
    }
});