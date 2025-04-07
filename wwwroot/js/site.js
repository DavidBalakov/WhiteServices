document.addEventListener("DOMContentLoaded", () => {
  var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
  var tooltipList = tooltipTriggerList.map((tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl))

  var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
  var popoverList = popoverTriggerList.map((popoverTriggerEl) => new bootstrap.Popover(popoverTriggerEl))

  const navbarToggler = document.querySelector(".navbar-toggler")
  if (navbarToggler) {
    navbarToggler.addEventListener("click", () => {
      document.body.classList.toggle("mobile-menu-open")
    })
  }

  window.addEventListener("scroll", () => {
    const navbar = document.querySelector(".navbar")
    if (navbar) {
      if (window.scrollY > 10) {
        navbar.classList.add("navbar-scrolled")
      } else {
        navbar.classList.remove("navbar-scrolled")
      }
    }
  })

  const statCards = document.querySelectorAll(".stat-card")
  if (statCards.length > 0) {
    statCards.forEach((card, index) => {
      setTimeout(() => {
        card.classList.add("animated")
      }, index * 100)
    })
  }

  var carouselElements = document.querySelectorAll(".carousel")
  if (carouselElements.length > 0) {
    carouselElements.forEach((carouselEl) => {
      var carousel = new bootstrap.Carousel(carouselEl, {
        interval: 5000,
        wrap: true,
      })
    })
  }

  var alerts = document.querySelectorAll(".alert")
  if (alerts.length > 0) {
    alerts.forEach((alert) => {
      setTimeout(() => {
        var bsAlert = new bootstrap.Alert(alert)
        bsAlert.close()
      }, 5000)
    })
  }

  var dropdownToggleList = [].slice.call(document.querySelectorAll(".dropdown-toggle"))
  dropdownToggleList.map((dropdownToggle) => new bootstrap.Dropdown(dropdownToggle))
})

function initializeTableSearch(tableId, searchInputId) {
  const searchInput = document.getElementById(searchInputId)
  const table = document.getElementById(tableId)

  if (searchInput && table) {
    searchInput.addEventListener("keyup", function () {
      const searchText = this.value.toLowerCase()
      const rows = table.querySelectorAll("tbody tr")

      rows.forEach((row) => {
        const text = row.textContent.toLowerCase()
        if (text.indexOf(searchText) > -1) {
          row.style.display = ""
        } else {
          row.style.display = "none"
        }
      })
    })
  }
}

function applyTableFilters(tableId, filters) {
  const table = document.getElementById(tableId)
  if (!table) return

  const rows = table.querySelectorAll("tbody tr")

  rows.forEach((row) => {
    let showRow = true

    for (const filter in filters) {
      if (filters[filter]) {
        const cellValue = row.querySelector(`td[data-${filter}]`)?.getAttribute(`data-${filter}`) || ""
        if (cellValue !== filters[filter]) {
          showRow = false
          break
        }
      }
    }

    row.style.display = showRow ? "" : "none"
  })
}
