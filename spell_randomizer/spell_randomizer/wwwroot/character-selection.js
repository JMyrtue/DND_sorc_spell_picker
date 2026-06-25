document.addEventListener('DOMContentLoaded', () => {
    const createButton = document.getElementById('create-button');
    const loadButton = document.getElementById('load-button');
    const newCharacterNameInput = document.getElementById('new-character-name');
    const existingCharactersSelect = document.getElementById('existing-characters');

    // Fetch existing characters and populate the dropdown
    fetch('/api/characters')
        .then(response => response.json())
        .then(characters => {
            characters.forEach(name => {
                const option = document.createElement('option');
                option.value = name;
                option.textContent = name;
                existingCharactersSelect.appendChild(option);
            });
        });

    createButton.addEventListener('click', () => {
        const name = newCharacterNameInput.value;
        if (!name) {
            alert('Please enter a name for your new character.');
            return;
        }
        fetch('/api/character/new', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: name })
        }).then(response => {
            if (response.ok) {
                window.location.href = 'main.html';
            } else {
                alert('Failed to create character.');
            }
        });
    });

    loadButton.addEventListener('click', () => {
        const name = existingCharactersSelect.value;
        if (!name) {
            alert('Please select a character to load.');
            return;
        }
        fetch('/api/character/load', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: name })
        }).then(response => {
            if (response.ok) {
                window.location.href = 'main.html';
            } else {
                alert('Failed to load character.');
            }
        });
    });
});
