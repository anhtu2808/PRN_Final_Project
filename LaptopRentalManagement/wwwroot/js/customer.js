// Customer UI JavaScript
document.addEventListener('DOMContentLoaded', function () {
    // Initialize components
    initializeSearch();
    initializeFilters();
    initializeWishlist();
    initializeCart();
    initializeLazyLoading();
    initializeAnimations();

    // Search functionality
    function initializeSearch() {
        const searchForm = document.querySelector('form[role="search"]');
        const searchInput = document.querySelector('.search-input');
        
        if (searchForm && searchInput) {
            // Real-time search suggestions (debounced)
            let searchTimeout;
            searchInput.addEventListener('input', function() {
                clearTimeout(searchTimeout);
                searchTimeout = setTimeout(() => {
                    if (this.value.length >= 2) {
                        showSearchSuggestions(this.value);
                    } else {
                        hideSearchSuggestions();
                    }
                }, 300);
            });

            // Hide suggestions when clicking outside
            document.addEventListener('click', function(e) {
                if (!e.target.closest('.search-container')) {
                    hideSearchSuggestions();
                }
            });
        }
    }

    function showSearchSuggestions(query) {
        // Simulate API call for search suggestions
        const suggestions = [
            'MacBook Pro',
            'Dell XPS 13',
            'HP EliteBook',
            'Lenovo ThinkPad',
            'ASUS ZenBook'
        ].filter(item => item.toLowerCase().includes(query.toLowerCase()));

        const container = document.querySelector('.search-container');
        let dropdown = container.querySelector('.search-suggestions');
        
        if (!dropdown) {
            dropdown = document.createElement('div');
            dropdown.className = 'search-suggestions';
            container.appendChild(dropdown);
        }

        dropdown.innerHTML = suggestions.map(suggestion => 
            `<div class="suggestion-item" data-value="${suggestion}">${suggestion}</div>`
        ).join('');

        // Add click handlers for suggestions
        dropdown.querySelectorAll('.suggestion-item').forEach(item => {
            item.addEventListener('click', function() {
                document.querySelector('.search-input').value = this.dataset.value;
                hideSearchSuggestions();
                // Optionally trigger search
                document.querySelector('form[role="search"]').submit();
            });
        });

        dropdown.style.display = 'block';
    }

    function hideSearchSuggestions() {
        const dropdown = document.querySelector('.search-suggestions');
        if (dropdown) {
            dropdown.style.display = 'none';
        }
    }

    // Filters functionality
    function initializeFilters() {
        const filterInputs = document.querySelectorAll('.filter-input');
        const priceRangeInputs = document.querySelectorAll('.price-input');
        
        filterInputs.forEach(input => {
            input.addEventListener('change', applyFilters);
        });

        priceRangeInputs.forEach(input => {
            input.addEventListener('input', debounce(applyFilters, 500));
        });
    }

    function applyFilters() {
        const filters = {
            category: getSelectedValues('category'),
            brand: getSelectedValues('brand'),
            priceMin: document.querySelector('#priceMin')?.value || '',
            priceMax: document.querySelector('#priceMax')?.value || '',
            available: document.querySelector('#availableOnly')?.checked || false
        };

        // Update URL with filters
        const url = new URL(window.location);
        Object.keys(filters).forEach(key => {
            if (filters[key]) {
                url.searchParams.set(key, filters[key]);
            } else {
                url.searchParams.delete(key);
            }
        });

        // Update page without reload
        window.history.pushState({}, '', url);
        
        // Filter laptop cards
        filterLaptopCards(filters);
    }

    function getSelectedValues(name) {
        const checkboxes = document.querySelectorAll(`input[name="${name}"]:checked`);
        return Array.from(checkboxes).map(cb => cb.value);
    }

    function filterLaptopCards(filters) {
        const cards = document.querySelectorAll('.laptop-card');
        
        cards.forEach(card => {
            const shouldShow = matchesFilters(card, filters);
            card.closest('.col')?.style.setProperty('display', shouldShow ? 'block' : 'none');
        });

        // Update results count
        const visibleCards = document.querySelectorAll('.laptop-card[style*="block"], .laptop-card:not([style])');
        updateResultsCount(visibleCards.length);
    }

    function matchesFilters(card, filters) {
        // Extract card data
        const category = card.dataset.category;
        const brand = card.dataset.brand;
        const price = parseFloat(card.dataset.price);
        const available = card.dataset.available === 'true';

        // Check category filter
        if (filters.category.length > 0 && !filters.category.includes(category)) {
            return false;
        }

        // Check brand filter
        if (filters.brand.length > 0 && !filters.brand.includes(brand)) {
            return false;
        }

        // Check price range
        if (filters.priceMin && price < parseFloat(filters.priceMin)) {
            return false;
        }
        if (filters.priceMax && price > parseFloat(filters.priceMax)) {
            return false;
        }

        // Check availability
        if (filters.available && !available) {
            return false;
        }

        return true;
    }

    function updateResultsCount(count) {
        const countElement = document.querySelector('.results-count');
        if (countElement) {
            countElement.textContent = `${count} laptops found`;
        }
    }

    // Wishlist functionality
    function initializeWishlist() {
        const wishlistButtons = document.querySelectorAll('.btn-wishlist');
        
        wishlistButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                toggleWishlist(this);
            });
        });
    }

    function toggleWishlist(button) {
        const laptopId = button.dataset.laptopId;
        const isInWishlist = button.classList.contains('active');
        
        if (isInWishlist) {
            removeFromWishlist(laptopId);
            button.classList.remove('active');
            button.innerHTML = '<i class="far fa-heart"></i>';
            showToast('Removed from wishlist', 'info');
        } else {
            addToWishlist(laptopId);
            button.classList.add('active');
            button.innerHTML = '<i class="fas fa-heart"></i>';
            showToast('Added to wishlist', 'success');
        }
    }

    function addToWishlist(laptopId) {
        const wishlist = getWishlist();
        if (!wishlist.includes(laptopId)) {
            wishlist.push(laptopId);
            localStorage.setItem('wishlist', JSON.stringify(wishlist));
        }
    }

    function removeFromWishlist(laptopId) {
        const wishlist = getWishlist();
        const index = wishlist.indexOf(laptopId);
        if (index > -1) {
            wishlist.splice(index, 1);
            localStorage.setItem('wishlist', JSON.stringify(wishlist));
        }
    }

    function getWishlist() {
        return JSON.parse(localStorage.getItem('wishlist') || '[]');
    }

    // Cart functionality
    function initializeCart() {
        const cartButtons = document.querySelectorAll('.btn-rent');
        
        cartButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                if (!this.href) {
                    e.preventDefault();
                    addToCart(this.dataset.laptopId);
                }
            });
        });

        updateCartBadge();
    }

    function addToCart(laptopId) {
        const cart = getCart();
        const existingItem = cart.find(item => item.laptopId === laptopId);
        
        if (existingItem) {
            showToast('Laptop is already in cart', 'warning');
        } else {
            cart.push({
                laptopId: laptopId,
                addedAt: new Date().toISOString()
            });
            localStorage.setItem('cart', JSON.stringify(cart));
            updateCartBadge();
            showToast('Added to cart', 'success');
        }
    }

    function getCart() {
        return JSON.parse(localStorage.getItem('cart') || '[]');
    }

    function updateCartBadge() {
        const badge = document.querySelector('.cart-badge');
        const cart = getCart();
        if (badge) {
            badge.textContent = cart.length;
            badge.style.display = cart.length > 0 ? 'inline-block' : 'none';
        }
    }

    // Lazy loading for images
    function initializeLazyLoading() {
        const images = document.querySelectorAll('img[data-src]');
        
        if ('IntersectionObserver' in window) {
            const imageObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        img.src = img.dataset.src;
                        img.classList.remove('lazy');
                        observer.unobserve(img);
                    }
                });
            });

            images.forEach(img => imageObserver.observe(img));
        } else {
            // Fallback for older browsers
            images.forEach(img => {
                img.src = img.dataset.src;
            });
        }
    }

    // Animations
    function initializeAnimations() {
        // Fade in elements when they come into view
        const animatedElements = document.querySelectorAll('.fade-in-on-scroll');
        
        if ('IntersectionObserver' in window) {
            const animationObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('fade-in');
                    }
                });
            }, { threshold: 0.1 });

            animatedElements.forEach(el => animationObserver.observe(el));
        }
    }

    // Utility functions
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    function showToast(message, type = 'info') {
        // Create toast element
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.innerHTML = `
            <div class="toast-body">
                <i class="fas fa-${getToastIcon(type)} me-2"></i>
                ${message}
            </div>
        `;

        // Add to page
        const container = getToastContainer();
        container.appendChild(toast);

        // Animate in
        setTimeout(() => toast.classList.add('show'), 100);

        // Remove after delay
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => container.removeChild(toast), 300);
        }, 3000);
    }

    function getToastIcon(type) {
        const icons = {
            success: 'check-circle',
            warning: 'exclamation-triangle',
            error: 'times-circle',
            info: 'info-circle'
        };
        return icons[type] || 'info-circle';
    }

    function getToastContainer() {
        let container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
        }
        return container;
    }

    // Public API
    window.CustomerUI = {
        addToWishlist,
        removeFromWishlist,
        addToCart,
        showToast,
        applyFilters
    };

    console.log('Customer UI initialized successfully');
});

// Add CSS for search suggestions and toasts
const additionalStyles = `
.search-suggestions {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: white;
    border: 1px solid var(--border-color);
    border-radius: var(--radius-md);
    box-shadow: var(--shadow-lg);
    z-index: 1000;
    max-height: 200px;
    overflow-y: auto;
    display: none;
}

.suggestion-item {
    padding: 0.75rem 1rem;
    cursor: pointer;
    border-bottom: 1px solid var(--border-color);
    transition: background-color 0.2s ease;
}

.suggestion-item:hover {
    background-color: var(--light-color);
}

.suggestion-item:last-child {
    border-bottom: none;
}

.toast-container {
    position: fixed;
    top: 20px;
    right: 20px;
    z-index: 9999;
}

.toast {
    background: white;
    border-radius: var(--radius-md);
    box-shadow: var(--shadow-lg);
    margin-bottom: 0.5rem;
    transform: translateX(100%);
    opacity: 0;
    transition: all 0.3s ease;
    min-width: 250px;
}

.toast.show {
    transform: translateX(0);
    opacity: 1;
}

.toast-body {
    padding: 1rem 1.5rem;
    display: flex;
    align-items: center;
}

.toast-success {
    border-left: 4px solid var(--success-color);
}

.toast-warning {
    border-left: 4px solid var(--warning-color);
}

.toast-error {
    border-left: 4px solid var(--danger-color);
}

.toast-info {
    border-left: 4px solid var(--primary-color);
}

.lazy {
    opacity: 0;
    transition: opacity 0.3s ease;
}

.lazy.loaded {
    opacity: 1;
}
`;

// Inject additional styles
const styleSheet = document.createElement('style');
styleSheet.textContent = additionalStyles;
document.head.appendChild(styleSheet); 