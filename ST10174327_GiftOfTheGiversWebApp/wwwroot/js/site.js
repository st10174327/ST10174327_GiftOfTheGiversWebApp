// Gift of the Givers Foundation - Enhanced JavaScript functionality

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {

    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Animate counters on scroll
    animateCounters();

    // Initialize smooth scrolling for anchor links
    initSmoothScrolling();

    // Initialize form enhancements
    initFormEnhancements();

    // Initialize donation calculator
    initDonationCalculator();

    // Initialize disaster reporting enhancements
    initDisasterReporting();

    // Initialize volunteer management enhancements
    initVolunteerManagement();
});

// Counter animation function
function animateCounters() {
    const counters = document.querySelectorAll('.stats-counter');

    counters.forEach(counter => {
        const target = parseInt(counter.textContent.replace(/[^\d]/g, ''));
        const increment = target / 100;
        let current = 0;

        const updateCounter = () => {
            if (current < target) {
                current += increment;
                counter.textContent = Math.ceil(current) + (counter.textContent.includes('+') ? '+' : '');
                setTimeout(updateCounter, 20);
            }
        };

        // Start animation when element is visible
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    updateCounter();
                    observer.unobserve(entry.target);
                }
            });
        });

        observer.observe(counter);
    });
}

// Smooth scrolling for anchor links
function initSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Form enhancements
function initFormEnhancements() {
    // Auto-hide alerts after 5 seconds
    document.querySelectorAll('.alert').forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });

    // Form validation enhancements
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();

                // Show validation feedback
                form.classList.add('was-validated');

                // Focus on first invalid field
                const firstInvalid = form.querySelector(':invalid');
                if (firstInvalid) {
                    firstInvalid.focus();
                }
            }
        });
    });

    // Real-time validation feedback
    document.querySelectorAll('input, select, textarea').forEach(field => {
        field.addEventListener('blur', function() {
            if (this.checkValidity()) {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
            } else {
                this.classList.remove('is-valid');
                this.classList.add('is-invalid');
            }
        });

        field.addEventListener('input', function() {
            if (this.checkValidity()) {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
            }
        });
    });
}

// Donation calculator functionality
function initDonationCalculator() {
    const calculator = document.getElementById('donation-calculator');
    if (calculator) {
        const amountInput = calculator.querySelector('#donation-amount');
        const typeSelect = calculator.querySelector('#donation-type');
        const resultDiv = calculator.querySelector('#calculator-result');

        function updateCalculation() {
            const amount = parseFloat(amountInput.value) || 0;
            const type = typeSelect.value;

            let impact = '';
            switch(type) {
                case 'food':
                    impact = `${Math.floor(amount / 50)} families can be fed for a week`;
                    break;
                case 'medical':
                    impact = `${Math.floor(amount / 100)} medical kits can be provided`;
                    break;
                case 'shelter':
                    impact = `${Math.floor(amount / 200)} emergency shelter kits can be supplied`;
                    break;
                case 'water':
                    impact = `${Math.floor(amount / 25)} water purification systems can be distributed`;
                    break;
                default:
                    impact = 'Your donation will make a real difference in disaster relief efforts';
            }

            resultDiv.innerHTML = `
                <div class="alert alert-info">
                    <h6><i class="fas fa-calculator me-2"></i>Impact Calculator</h6>
                    <p class="mb-0">${impact}</p>
                </div>
            `;
        }

        amountInput.addEventListener('input', updateCalculation);
        typeSelect.addEventListener('change', updateCalculation);
    }
}

// Disaster reporting enhancements
function initDisasterReporting() {
    // Auto-fill location based on user's IP (mock implementation)
    const locationField = document.querySelector('input[name="Location"]');
    if (locationField && !locationField.value) {
        // In a real implementation, you would use a geolocation API
        locationField.placeholder = 'Enter disaster location (city, province)';
    }

    // Auto-categorize based on description
    const descriptionField = document.querySelector('textarea[name="Description"]');
    const categoryField = document.querySelector('select[name="AidType"]');

    if (descriptionField && categoryField) {
        descriptionField.addEventListener('input', function() {
            const text = this.value.toLowerCase();

            if (text.includes('flood') || text.includes('water')) {
                categoryField.value = 'Emergency Water Supply';
            } else if (text.includes('fire') || text.includes('burn')) {
                categoryField.value = 'Emergency Shelter';
            } else if (text.includes('medical') || text.includes('health')) {
                categoryField.value = 'Medical Supplies';
            } else if (text.includes('food') || text.includes('hunger')) {
                categoryField.value = 'Food Relief';
            }
        });
    }
}

// Volunteer management enhancements
function initVolunteerManagement() {
    // Filter volunteers by skills
    const skillFilter = document.getElementById('skill-filter');
    const volunteerCards = document.querySelectorAll('.volunteer-card');

    if (skillFilter && volunteerCards.length > 0) {
        skillFilter.addEventListener('change', function() {
            const selectedSkill = this.value.toLowerCase();

            volunteerCards.forEach(card => {
                const skills = card.dataset.skills?.toLowerCase() || '';

                if (selectedSkill === '' || skills.includes(selectedSkill)) {
                    card.style.display = 'block';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    }

    // Task assignment functionality
    document.querySelectorAll('.assign-volunteer-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const volunteerId = this.dataset.volunteerId;
            const taskId = this.dataset.taskId;

            // Show confirmation modal
            const modal = document.getElementById('assignment-modal');
            if (modal) {
                modal.querySelector('.volunteer-name').textContent = this.dataset.volunteerName;
                modal.querySelector('.task-title').textContent = this.dataset.taskTitle;
                modal.querySelector('#assignment-volunteer-id').value = volunteerId;
                modal.querySelector('#assignment-task-id').value = taskId;

                const bsModal = new bootstrap.Modal(modal);
                bsModal.show();
            }
        });
    });
}

// Utility functions
function formatCurrency(amount) {
    return new Intl.NumberFormat('en-ZA', {
        style: 'currency',
        currency: 'ZAR'
    }).format(amount);
}

function formatDate(date) {
    return new Intl.DateTimeFormat('en-ZA', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(new Date(date));
}

// Progress bar animation
function animateProgressBar(selector, percentage, duration = 2000) {
    const progressBar = document.querySelector(selector);
    if (progressBar) {
        let current = 0;
        const increment = percentage / (duration / 16); // 60fps

        const timer = setInterval(() => {
            current += increment;
            if (current >= percentage) {
                current = percentage;
                clearInterval(timer);
            }
            progressBar.style.width = current + '%';
            progressBar.setAttribute('aria-valuenow', current);
        }, 16);
    }
}

// Loading spinner for forms
function showLoadingSpinner(button) {
    if (button) {
        button.disabled = true;
        button.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Processing...';
    }
}

function hideLoadingSpinner(button, originalText) {
    if (button) {
        button.disabled = false;
        button.innerHTML = originalText;
    }
}

// Toast notifications
function showToast(message, type = 'info', duration = 3000) {
    const toastContainer = document.getElementById('toast-container') || createToastContainer();

    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type} border-0`;
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-triangle' : 'info-circle'} me-2"></i>
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    toastContainer.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast, { delay: duration });
    bsToast.show();

    toast.addEventListener('hidden.bs.toast', () => {
        toast.remove();
    });
}

function createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toast-container';
    container.className = 'toast-container position-fixed top-0 end-0 p-3';
    container.style.zIndex = '9999';
    document.body.appendChild(container);
    return container;
}

// Export functions for use in other scripts
window.GiftOfTheGivers = {
    formatCurrency,
    formatDate,
    animateProgressBar,
    showLoadingSpinner,
    hideLoadingSpinner,
    showToast
};