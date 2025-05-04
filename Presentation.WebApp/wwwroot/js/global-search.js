function handleSearch(event) {
    event.preventDefault();
    const searchTerm = document.getElementById('globalSearch').value.toLowerCase();
    const currentPage = document.querySelector('.header-container h2').textContent.toLowerCase();

    // projects filtering
    if (currentPage === 'projects') {

        // set "all" as active table when searching
        document.querySelectorAll('.tab-item').forEach(tab => {
            tab.classList.remove('active')
        })
        document.querySelector('[data-project-tab-filter="all"]').classList.add('active');

        const projectCards = document.querySelectorAll('.project-list .project');
        projectCards.forEach(projectCard => {

            const projectName = (projectCard.querySelector('.project-title')?.textContent || '').toLowerCase();
            const projectDescription = (projectCard.querySelector('.description-text')?.textContent || '').toLowerCase();
            const clientName = (projectCard.querySelector('.project-company')?.textContent || '').toLowerCase();

            if (projectName.includes(searchTerm) ||
                projectDescription.includes(searchTerm) ||
                clientName.includes(searchTerm)) {
                projectCard.style.display = '';
            } else {
                projectCard.style.display = 'none';
            }
        });
    }

    // members filtering
    else if (currentPage === 'members') {
        const memberCards = document.querySelectorAll('.member-list .card');
        memberCards.forEach(memberCard => {
            const memberName = (memberCard.querySelector('.name h4')?.textContent || '').toLowerCase();
            const jobTitle = (memberCard.querySelector('.job-title')?.textContent || '').toLowerCase();
            const email = (memberCard.querySelector('.email')?.textContent || '').toLowerCase();
            const phone = (memberCard.querySelector('.phoneNumber')?.textContent || '').toLowerCase();

            if (memberName.includes(searchTerm) ||
                jobTitle.includes(searchTerm) ||
                email.includes(searchTerm) ||
                phone.includes(searchTerm)) {
                memberCard.style.display = '';
            } else {
                memberCard.style.display = 'none';
            }
        });
    }

    // clients filtering
    else if (currentPage === 'clients') {
        const clients = document.querySelectorAll('.client-list tbody tr');
        clients.forEach(client => {
            const clientName = (client.querySelector('.client-name')?.textContent || '').toLowerCase();
            const email = (client.querySelector('.client-email')?.textContent || '').toLowerCase();
            const location = (client.querySelector('.client-location')?.textContent || '').toLowerCase();
            const phone = (client.querySelector('.client-phone')?.textContent || '').toLowerCase();

            if (clientName.includes(searchTerm) ||
                email.includes(searchTerm) ||
                location.includes(searchTerm) ||
                phone.includes(searchTerm)) {
                client.style.display = '';
            } else {
                client.style.display = 'none';
            }
        });
    }

    return false;
}

// event listener for real-time filtering
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('globalSearch');
    if (searchInput) {
        searchInput.addEventListener('input', function () {
            handleSearch(new Event('input'));
        });
    }
});