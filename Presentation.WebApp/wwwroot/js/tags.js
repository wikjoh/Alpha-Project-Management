function initTagSelector(config) {
    let activeIndex = -1;

    const tagContainer = document.getElementById(config.containerId);
    const input = document.getElementById(config.inputId);
    const results = document.getElementById(config.resultsId);

    let selectedInputIdsContainer = document.createElement('div');
    selectedInputIdsContainer.classList.add('selected-input-ids-container');
    selectedInputIdsContainer.setAttribute('style', 'display:none;');
    tagContainer.appendChild(selectedInputIdsContainer);
    selectedInputIdsContainer = Array.from(tagContainer.children).find(c => c.classList.contains('selected-input-ids-container'));

    if (Array.isArray(config.preselected)) {
        config.preselected.forEach(item => addTag(item));
    }

    input.addEventListener('focus', () => {
        tagContainer.classList.add('focused');
        results.classList.add('focused');
    });

    input.addEventListener('blur', () => {
        setTimeout(() => {
            tagContainer.classList.remove('focused');
            results.classList.remove('focused');
        }, 100);
    });

    let timeout;
    input.addEventListener('input', () => {
        clearTimeout(timeout);
        timeout = setTimeout(async (U) => {
            const query = input.value.trim();
            activeIndex = -1;

            if (query.length === 0) {
                results.style.display = 'none';
                results.innerHTML = '';
                return;
            }

            fetch(config.searchUrl(query))
                .then(r => r.json())
                .then(data => renderSearchResults(data));
        }, 300);
    });

    input.addEventListener('keydown', (e) => {
        const items = results.querySelectorAll('.search-item');

        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex + 1) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'ArrowUp':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex - 1 + items.length) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'Enter':
                e.preventDefault();
                if (activeIndex >= 0 && items[activeIndex]) {
                    items[activeIndex].click();
                }
                break;

            case 'Backspace':
                if (input.value === '') {
                    removeLastTag();
                }
                break;

            // Hide results and clear input when pressing escape
            case 'Escape':
                clearResults();
        }
    });

    // Hide results and clear input when clicking outside tagContainer or results
    document.addEventListener('click', (e) => {
        if (!tagContainer.contains(e.target) && !results.contains(e.target)) {
            clearResults();
        }
    })

    function clearResults() {
        results.style.display = 'none';
        results.innerHTML = '';
        input.value = '';
    }

    function updateActiveItem(items) {
        items.forEach(item => item.classList.remove('active'));
        if (items[activeIndex]) {
            items[activeIndex].classList.add('active');
            items[activeIndex].scrollIntoView({ block: 'nearest' });
        }
    }

    function renderSearchResults(data) {
        results.innerHTML = '';

        if (data.length === 0) {
            const noResult = document.createElement('div');
            noResult.classList.add('search-item');
            noResult.textContent = config.emptyMessage || 'No results.';
            results.appendChild(noResult);
        } else {
            data.forEach(item => {
                if (!Array.from(selectedInputIdsContainer.querySelectorAll('input')).some(input => input.value == item.id)) {
                    const resultItem = document.createElement('div');
                    resultItem.classList.add('search-item');
                    resultItem.dataset.id = item.id;

                    if (config.tagClass === 'user-tag') {
                        resultItem.innerHTML = `
                            <img class="user-avatar" src="${config.avatarFolder || ''}${item[config.imageProperty]}">
                            <span>${item[config.displayProperty]}</span>
                            `;
                    } else {
                        resultItem.innerHTML = `
                            <span>${item[config.displayProperty]}</span>
                        `;
                    }

                    resultItem.addEventListener('click', () => addTag(item));
                    results.appendChild(resultItem);
                }
            });
        }

        results.style.display = 'block';
    }

    function addTag(item) {
        const idKeyName = config.idKeyName || 'id';
        const id = (config.idIsDataTypeInt) ? parseInt(item[idKeyName]) : item[idKeyName];

        if (Array.from(selectedInputIdsContainer.querySelectorAll('input')).some(input => input.value == id)) return;

        // if not multiSelect, wipe previous tags
        if (!config.multiSelect) singleSelectWipePreviousTags();

        let selectedInputItem = document.createElement('input');
        selectedInputItem.setAttribute("type", "text");
        selectedInputItem.setAttribute("name", [config.viewModelProperty]);
        selectedInputItem.setAttribute("value", id);
        selectedInputIdsContainer.appendChild(selectedInputItem);

        const tag = document.createElement('div');
        tag.classList.add(config.tagClass || 'tag');

        if (config.tagClass === 'user-tag') {
            tag.innerHTML = `
                <img class="user-avatar" src="${config.avatarFolder || ''}${item[config.imageProperty]}">
                <span>${item[config.displayProperty]}</span>
            `;
        } else {
            tag.innerHTML = `
            <span>${item[config.displayProperty]}</span>
            `;
        }

        const removeBtn = document.createElement('span');
        removeBtn.textContent = 'x';
        removeBtn.classList.add('btn-remove');
        removeBtn.dataset.id = id;
        removeBtn.addEventListener('click', (e) => {
            Array.from(selectedInputIdsContainer.querySelectorAll('input')).find(input => input.value == id).remove();
            tag.remove();
            e.stopPropagation();
        });

        tag.appendChild(removeBtn);
        tagContainer.insertBefore(tag, input);

        input.value = '';
        results.innerHTML = '';
        results.style.display = 'none';
    }

    function removeLastTag() {
        const tags = tagContainer.querySelectorAll(`.${config.tagClass}`);
        if (tags.length === 0) return;

        const lastTag = tags[tags.length - 1];
        const lastId = parseInt(lastTag.querySelector('.btn-remove').dataset.id);

        Array.from(selectedInputIdsContainer.querySelectorAll('input')).find(input => input.value == lastId).remove();
        lastTag.remove();
    }

    function singleSelectWipePreviousTags() {
        tagContainer.querySelectorAll('.tag').forEach(e => {
            e.remove();
        });
        selectedInputIdsContainer.innerHTML = '';
    }
}