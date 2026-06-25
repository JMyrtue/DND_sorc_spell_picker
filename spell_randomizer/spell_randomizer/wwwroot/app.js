document.addEventListener('DOMContentLoaded', () => {
    fetchCharacterData();

    document.getElementById('metamagic-button').addEventListener('click', useMetamagic);
    document.getElementById('rest-button').addEventListener('click', rest);
    document.getElementById('level-up-button').addEventListener('click', levelUp);
    document.getElementById('level-down-button').addEventListener('click', levelDown);
    document.getElementById('save-button').addEventListener('click', saveCharacter);
});

async function fetchCharacterData() {
    try {
        const response = await fetch('/api/character');
        
        if (response.status === 404) {
            window.location.href = 'index.html';
            return;
        }

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        updateCharacterInfo(data);
        updateCantrips(data.cantrips);
        updateSpells(data.spells);
        updateSpellSlots(data.spellSlots);
        
        // Adjust UI after data is loaded and rendered
        setTimeout(adjustDescriptionBoxHeight, 0);
    } catch (error) {
        console.error('Could not fetch character data:', error);
    }
}

function adjustDescriptionBoxHeight() {
    const leftColumn = document.getElementById('character-sheet');
    const actionsBox = document.getElementById('actions');
    const descriptionBox = document.getElementById('spell-description');
    const descriptionContainer = document.getElementById('spell-description-container');

    // Total height of the left column
    const leftHeight = leftColumn.offsetHeight;
    
    // Height of the elements *above* the description container in the right column
    const actionsHeight = actionsBox.offsetHeight;
    
    // Calculate the available height for the description container
    // We subtract the height of the actions box and a buffer for margins/padding
    const availableHeight = leftHeight - actionsHeight - 120; // Adjusted buffer

    descriptionContainer.style.height = `${availableHeight}px`;
    descriptionBox.style.height = '100%'; // Make the inner box fill the container
}


function updateCharacterInfo(data) {
    document.getElementById('character-name').textContent = data.name;
    document.getElementById('character-level').textContent = data.level;
    document.getElementById('sorcery-points').textContent = data.remainingSorceryPoints;
    document.getElementById('wild-magic-counter').textContent = data.wildMagicCounter;
}

function updateCantrips(cantrips) {
    const cantripList = document.getElementById('cantrip-list');
    cantripList.innerHTML = '';
    cantrips.forEach(spell => {
        const li = document.createElement('li');
        li.textContent = spell.name;
        li.addEventListener('mouseenter', () => showSpellDescription(spell));
        cantripList.appendChild(li);
    });
}

function updateSpells(spells) {
    const spellList = document.getElementById('spell-list');
    spellList.innerHTML = '';
    let lastLevel = -1;

    spells.forEach(spell => {
        if (spell.level !== lastLevel) {
            const header = document.createElement('li');
            header.classList.add('spell-level-header');
            header.textContent = `Level ${spell.level}`;
            spellList.appendChild(header);
            lastLevel = spell.level;
        }

        const li = document.createElement('li');
        li.textContent = spell.name;
        li.addEventListener('mouseenter', () => showSpellDescription(spell));
        spellList.appendChild(li);
    });
}

function updateSpellSlots(spellSlots) {
    const spellSlotsDiv = document.getElementById('spell-slots');
    const castSpellButtonsDiv = document.getElementById('cast-spell-buttons');
    
    spellSlotsDiv.innerHTML = '<h2>Spell Slots</h2>';
    castSpellButtonsDiv.innerHTML = '<h3>Cast a Spell</h3>';

    for (const level in spellSlots) {
        if (spellSlots.hasOwnProperty(level)) {
            const slotInfo = spellSlots[level];
            const levelNum = parseInt(level.replace('Level ', ''));

            const p = document.createElement('p');
p.classList.add('spell-slot-row');

const text = document.createElement('span');
text.textContent = `${level}: ${slotInfo.available} of ${slotInfo.total}`;
p.appendChild(text);

if (levelNum <= 5) {
    const minusButton = document.createElement('button');
    minusButton.textContent = '-';
    minusButton.classList.add('plus-minus-button');
    minusButton.addEventListener('click', () => convertSlotsToPoints(levelNum));
    p.appendChild(minusButton);

    const plusButton = document.createElement('button');
    plusButton.textContent = '+';
    plusButton.classList.add('plus-minus-button');
    plusButton.addEventListener('click', () => convertPointsToSlots(levelNum));
    p.appendChild(plusButton);
}

spellSlotsDiv.appendChild(p);

            const button = document.createElement('button');
            button.textContent = `Cast Level ${levelNum}`;
            button.addEventListener('click', () => handleCastSpellClick(levelNum));
            if (slotInfo.available === 0) {
                button.disabled = true;
            }
            castSpellButtonsDiv.appendChild(button);
        }
    }
}

async function showSpellDescription(spell) {
    const spellDescriptionDiv = document.getElementById('spell-description');
    let description = 'No description available.';

    if (spell.description) {
        description = spell.description;
    } else {
        try {
            const response = await fetch(`/api/spell/${encodeURIComponent(spell.name)}`);
            if (response.ok) {
                const spellDetails = await response.json();
                description = spellDetails.description;
                spell.description = description;
            }
        } catch (error) {
            console.error('Could not fetch spell description:', error);
        }
    }

    spellDescriptionDiv.innerHTML = `<h3>${spell.name}</h3>${description}`;
}

function askWildMagicSurge(currentCounter) {
    return new Promise((resolve) => {
        const modal = document.getElementById('surge-modal');
        const textElement = document.getElementById('surge-modal-text');
        const yesButton = document.getElementById('surge-yes-button');
        const noButton = document.getElementById('surge-no-button');

        textElement.innerHTML = `Your current Wild Magic Surge Threshold is <strong>${currentCounter}</strong>.<br><br>Did a Wild Magic Surge occur on this cast?`;
        modal.style.display = 'flex';

        const handleChoice = (result) => {
            modal.style.display = 'none';
            yesButton.removeEventListener('click', onYes);
            noButton.removeEventListener('click', onNo);
            resolve(result);
        };

        const onYes = () => handleChoice(true);
        const onNo = () => handleChoice(false);

        yesButton.addEventListener('click', onYes);
        noButton.addEventListener('click', onNo);
    });
}

async function handleCastSpellClick(level) {
    if (!level) return;

    const currentCounter = document.getElementById('wild-magic-counter').textContent;
    const surgeOccurred = await askWildMagicSurge(currentCounter);

    try {
        const response = await fetch('/api/cast', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ 
                level: level,
                wildMagicSurgeOccurred: surgeOccurred
            })
        });
        if (response.ok) {
            fetchCharacterData();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error casting spell:', error);
    }
}


async function useMetamagic() {
    const cost = document.getElementById('metamagic-cost').value;
    if (cost) {
        try {
            const response = await fetch('/api/metamagic', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ cost: parseInt(cost) })
            });
            if (response.ok) {
                fetchCharacterData();
            } else {
                const error = await response.text();
                alert(`Error: ${error}`);
            }
        } catch (error) {
            console.error('Error using metamagic:', error);
        }
    }
}

async function rest() {
    try {
        const response = await fetch('/api/rest', { method: 'POST' });
        if (response.ok) {
            fetchCharacterData();
        } else {
            alert('Error during rest.');
        }
    } catch (error) {
        console.error('Error during rest:', error);
    }
}

async function levelUp() {
    try {
        const response = await fetch('/api/levelup', { method: 'POST' });
        if (response.ok) {
            fetchCharacterData();
        } else {
            alert('Error leveling up.');
        }
    } catch (error) {
        console.error('Error leveling up:', error);
    }
}

async function levelDown() {
    try {
        const response = await fetch('/api/leveldown', { method: 'POST' });
        if (response.ok) {
            fetchCharacterData();
        } else {
            alert('Error leveling down.');
        }
    } catch (error) {
        console.error('Error leveling down:', error);
    }
}

async function convertPointsToSlots(level) {
    try {
        const response = await fetch('/api/convert/pointstoslots', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ level: level })
        });
        if (response.ok) {
            fetchCharacterData();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error converting points to slots:', error);
    }
}

async function convertSlotsToPoints(level) {
    try {
        const response = await fetch('/api/convert/slotstopoints', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ level: level })
        });
        if (response.ok) {
            fetchCharacterData();
        } else {
            const error = await response.text();
            alert(`Error: ${error}`);
        }
    } catch (error) {
        console.error('Error converting slots to points:', error);
    }
}

async function saveCharacter() {
    try {
        const response = await fetch('/api/character/save', { method: 'POST' });
        if (response.ok) {
            alert('Character saved!');
        } else {
            alert('Error saving character.');
        }
    } catch (error) {
        console.error('Error saving character:', error);
    }
}
