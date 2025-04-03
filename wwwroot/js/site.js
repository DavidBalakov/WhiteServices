<<<<<<< HEAD
﻿// site.js - Main JavaScript file for the Appliance Repair Management System

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
    
    // Initialize carousels
    var carouselList = [].slice.call(document.querySelectorAll('.carousel'));
    carouselList.forEach(function(carouselEl) {
        var carousel = new bootstrap.Carousel(carouselEl, {
            interval: 5000,
            wrap: true,
            touch: true
        });
    });
    
    // Table search functionality
    const tableSearchInputs = document.querySelectorAll('.table-search');
    tableSearchInputs.forEach(input => {
        input.addEventListener('keyup', function() {
            const searchTerm = this.value.toLowerCase();
            const tableId = this.getAttribute('data-table-target');
            const table = document.getElementById(tableId);
            
            if (!table) return;
            
            const rows = table.querySelectorAll('tbody tr');
            
            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                if (text.includes(searchTerm)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        });
    });
    
    // Table sorting functionality
    const sortableHeaders = document.querySelectorAll('th[data-sort]');
    sortableHeaders.forEach(header => {
        header.addEventListener('click', function() {
            const tableId = this.closest('table').id;
            const columnIndex = Array.from(this.parentNode.children).indexOf(this);
            const sortDirection = this.getAttribute('data-sort-direction') === 'asc' ? 'desc' : 'asc';
            
            // Update sort direction attribute
            sortableHeaders.forEach(h => h.setAttribute('data-sort-direction', ''));
            this.setAttribute('data-sort-direction', sortDirection);
            
            // Update sort icons
            sortableHeaders.forEach(h => {
                h.querySelector('.sort-icon')?.classList.remove('bi-arrow-up', 'bi-arrow-down');
                h.querySelector('.sort-icon')?.classList.add('bi-arrow-down-up');
            });
            
            const sortIcon = this.querySelector('.sort-icon');
            if (sortIcon) {
                sortIcon.classList.remove('bi-arrow-down-up');
                sortIcon.classList.add(sortDirection === 'asc' ? 'bi-arrow-up' : 'bi-arrow-down');
            }
            
            // Sort the table
            sortTable(tableId, columnIndex, sortDirection);
        });
    });
    
    // Function to sort table
    function sortTable(tableId, columnIndex, direction) {
        const table = document.getElementById(tableId);
        if (!table) return;
        
        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));
        
        // Sort rows
        rows.sort((a, b) => {
            const aValue = a.cells[columnIndex].textContent.trim();
            const bValue = b.cells[columnIndex].textContent.trim();
            
            // Check if values are dates
            const aDate = new Date(aValue);
            const bDate = new Date(bValue);
            
            if (!isNaN(aDate) && !isNaN(bDate)) {
                return direction === 'asc' ? aDate - bDate : bDate - aDate;
            }
            
            // Check if values are numbers
            const aNum = parseFloat(aValue);
            const bNum = parseFloat(bValue);
            
            if (!isNaN(aNum) && !isNaN(bNum)) {
                return direction === 'asc' ? aNum - bNum : bNum - aNum;
            }
            
            // Default to string comparison
            return direction === 'asc' 
                ? aValue.localeCompare(bValue) 
                : bValue.localeCompare(aValue);
        });
        
        // Reorder rows in the table
        rows.forEach(row => tbody.appendChild(row));
    }
    
    // Filter functionality
    const filterSelects = document.querySelectorAll('select[data-filter]');
    filterSelects.forEach(select => {
        select.addEventListener('change', function() {
            applyFilters();
        });
    });
    
    function applyFilters() {
        const tableId = document.querySelector('select[data-filter]')?.getAttribute('data-table-target');
        if (!tableId) return;
        
        const table = document.getElementById(tableId);
        if (!table) return;
        
        const rows = table.querySelectorAll('tbody tr');
        const filters = {};
        
        // Collect all filter values
        filterSelects.forEach(select => {
            const filterName = select.getAttribute('data-filter');
            const filterValue = select.value.toLowerCase();
            if (filterValue) {
                filters[filterName] = filterValue;
            }
        });
        
        // Apply filters
        rows.forEach(row => {
            let showRow = true;
            
            Object.keys(filters).forEach(filterName => {
                const filterValue = filters[filterName];
                const cellIndex = parseInt(filterName.replace('column-', ''));
                
                if (!isNaN(cellIndex) && row.cells[cellIndex]) {
                    const cellValue = row.cells[cellIndex].textContent.toLowerCase();
                    if (!cellValue.includes(filterValue)) {
                        showRow = false;
                    }
                }
            });
            
            row.style.display = showRow ? '' : 'none';
        });
    }
    
    // Reset filters
    const resetFilterBtn = document.getElementById('resetFilters');
    if (resetFilterBtn) {
        resetFilterBtn.addEventListener('click', function() {
            filterSelects.forEach(select => {
                select.value = '';
            });
            
            const tableId = document.querySelector('select[data-filter]')?.getAttribute('data-table-target');
            if (tableId) {
                const table = document.getElementById(tableId);
                if (table) {
                    const rows = table.querySelectorAll('tbody tr');
                    rows.forEach(row => {
                        row.style.display = '';
                    });
                }
            }
            
            // Also reset search
            const searchInput = document.querySelector('.table-search');
            if (searchInput) {
                searchInput.value = '';
            }
        });
    }
    
    // Handle status badge colors
    const statusBadges = document.querySelectorAll('.status-badge');
    statusBadges.forEach(badge => {
        const status = badge.textContent.toLowerCase();
        
        if (status.includes('pending') || status.includes('чакащ')) {
            badge.classList.add('pending');
        } else if (status.includes('progress') || status.includes('процес')) {
            badge.classList.add('in-progress');
        } else if (status.includes('complete') || status.includes('завършен')) {
            badge.classList.add('completed');
        } else if (status.includes('cancel') || status.includes('отказан')) {
            badge.classList.add('cancelled');
        }
    });
});
=======
﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
>>>>>>> 6fa8649bceae9423631925b6c9382e311f30698f
