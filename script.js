// Focus Planner JavaScript

// State management
let tasks = [];
let currentFilter = 'all';

// DOM Elements
const taskInput = document.getElementById('taskInput');
const addBtn = document.getElementById('addBtn');
const taskList = document.getElementById('taskList');
const filterBtns = document.querySelectorAll('.filter-btn');
const clearCompletedBtn = document.getElementById('clearCompleted');
const clearAllBtn = document.getElementById('clearAll');
const totalCount = document.getElementById('totalCount');
const activeCount = document.getElementById('activeCount');
const completedCount = document.getElementById('completedCount');
const exportJSONBtn = document.getElementById('exportJSON');
const importJSONInput = document.getElementById('importJSON');
const loadDailyBasicBtn = document.getElementById('loadDailyBasic');
const loadDailyLongformBtn = document.getElementById('loadDailyLongform');

// Load tasks from localStorage
function loadTasks() {
    const savedTasks = localStorage.getItem('focusPlannerTasks');
    if (savedTasks) {
        tasks = JSON.parse(savedTasks);
    }
    renderTasks();
}

// Save tasks to localStorage
function saveTasks() {
    try {
        localStorage.setItem('focusPlannerTasks', JSON.stringify(tasks));
        showSaveFeedback();
        return true;
    } catch (e) {
        console.error('저장 실패:', e);
        showSaveFeedback(false);
        return false;
    }
}

// Show save feedback
function showSaveFeedback(success = true) {
    const saveStatus = document.getElementById('saveStatus');
    if (!saveStatus) return;
    
    if (success) {
        saveStatus.classList.add('saved');
        saveStatus.querySelector('.save-text').textContent = '저장됨 ✓';
        
        setTimeout(() => {
            saveStatus.classList.remove('saved');
            saveStatus.querySelector('.save-text').textContent = '자동 저장됨';
        }, 2000);
    } else {
        saveStatus.classList.add('error');
        saveStatus.querySelector('.save-text').textContent = '저장 실패';
        
        setTimeout(() => {
            saveStatus.classList.remove('error');
            saveStatus.querySelector('.save-text').textContent = '자동 저장됨';
        }, 3000);
    }
}

// Add new task
function addTask() {
    const text = taskInput.value.trim();
    if (text === '') {
        alert('할 일을 입력해주세요!');
        return;
    }

    const newTask = {
        id: Date.now(),
        text: text,
        completed: false,
        createdAt: new Date().toISOString()
    };

    tasks.unshift(newTask);
    taskInput.value = '';
    saveTasks();
    renderTasks();
    updateStats();
}

// Toggle task completion
function toggleTask(id) {
    const task = tasks.find(t => t.id === id);
    if (task) {
        task.completed = !task.completed;
        saveTasks();
        renderTasks();
        updateStats();
    }
}

// Delete task
function deleteTask(id) {
    if (confirm('이 할 일을 삭제하시겠습니까?')) {
        tasks = tasks.filter(t => t.id !== id);
        saveTasks();
        renderTasks();
        updateStats();
    }
}

// Filter tasks
function getFilteredTasks() {
    switch (currentFilter) {
        case 'active':
            return tasks.filter(t => !t.completed);
        case 'completed':
            return tasks.filter(t => t.completed);
        default:
            return tasks;
    }
}

// Render tasks
function renderTasks() {
    const filteredTasks = getFilteredTasks();
    
    if (filteredTasks.length === 0) {
        taskList.innerHTML = `
            <li class="empty-state">
                <div class="empty-state-icon">📝</div>
                <p>${currentFilter === 'all' ? '할 일을 추가해보세요!' : 
                     currentFilter === 'active' ? '진행중인 할 일이 없습니다.' : 
                     '완료된 할 일이 없습니다.'}</p>
            </li>
        `;
        return;
    }

    taskList.innerHTML = filteredTasks.map(task => `
        <li class="task-item ${task.completed ? 'completed' : ''}">
            <input 
                type="checkbox" 
                class="task-checkbox" 
                ${task.completed ? 'checked' : ''} 
                onchange="toggleTask(${task.id})"
            >
            <span class="task-text">${escapeHtml(task.text)}</span>
            <button class="task-delete" onclick="deleteTask(${task.id})">삭제</button>
        </li>
    `).join('');
}

// Update statistics
function updateStats() {
    totalCount.textContent = tasks.length;
    activeCount.textContent = tasks.filter(t => !t.completed).length;
    completedCount.textContent = tasks.filter(t => t.completed).length;
}

// Escape HTML to prevent XSS
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Export tasks to JSON file
function exportToJSON() {
    if (tasks.length === 0) {
        alert('내보낼 할 일이 없습니다.');
        return;
    }

    const data = {
        version: '1.0',
        exportDate: new Date().toISOString(),
        tasks: tasks
    };

    const jsonString = JSON.stringify(data, null, 2);
    const blob = new Blob([jsonString], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    
    const dateStr = new Date().toISOString().split('T')[0];
    link.href = url;
    link.download = `focus-planner-${dateStr}.json`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);

    alert(`할 일 ${tasks.length}개가 JSON 파일로 저장되었습니다!\n\n파일 위치: 다운로드 폴더\n파일명: focus-planner-${dateStr}.json`);
}

// Import tasks from JSON file
function importFromJSON(event) {
    const file = event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = function(e) {
        try {
            const data = JSON.parse(e.target.result);
            
            // Support both old format (array) and new format (object with tasks)
            let importedTasks = Array.isArray(data) ? data : (data.tasks || []);
            
            if (!Array.isArray(importedTasks) || importedTasks.length === 0) {
                alert('유효한 할 일 데이터가 없습니다.');
                return;
            }

            // Validate task structure
            const validTasks = importedTasks.filter(task => 
                task && typeof task === 'object' && 
                task.id && task.text && typeof task.text === 'string'
            );

            if (validTasks.length === 0) {
                alert('유효한 할 일 형식이 아닙니다.');
                return;
            }

            if (confirm(`기존 ${tasks.length}개의 할 일과 합치시겠습니까?\n(취소하면 기존 데이터를 대체합니다)`)) {
                // Merge: combine with existing tasks
                const existingIds = new Set(tasks.map(t => t.id));
                const newTasks = validTasks.filter(t => !existingIds.has(t.id));
                tasks = [...tasks, ...newTasks];
            } else {
                // Replace: use imported tasks only
                tasks = validTasks;
            }

            saveTasks();
            renderTasks();
            updateStats();
            alert(`할 일 ${validTasks.length}개를 성공적으로 가져왔습니다!`);
        } catch (error) {
            console.error('JSON 파싱 오류:', error);
            alert('JSON 파일을 읽는 중 오류가 발생했습니다.\n파일 형식을 확인해주세요.');
        }
    };

    reader.onerror = function() {
        alert('파일을 읽는 중 오류가 발생했습니다.');
    };

    reader.readAsText(file);
    event.target.value = ''; // Reset input to allow importing same file again
}

// Load template from JSON file
async function loadTemplate(filename) {
    try {
        const response = await fetch(filename);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        
        // Support both old format (array) and new format (object with tasks)
        let importedTasks = Array.isArray(data) ? data : (data.tasks || []);
        
        if (!Array.isArray(importedTasks) || importedTasks.length === 0) {
            alert('템플릿에 유효한 할 일 데이터가 없습니다.');
            return;
        }

        // Validate task structure
        const validTasks = importedTasks.filter(task => 
            task && typeof task === 'object' && 
            task.id && task.text && typeof task.text === 'string'
        );

        if (validTasks.length === 0) {
            alert('유효한 할 일 형식이 아닙니다.');
            return;
        }

        // Generate unique IDs to avoid conflicts
        const maxExistingId = tasks.length > 0 ? Math.max(...tasks.map(t => parseInt(t.id) || 0)) : 0;
        validTasks.forEach((task, index) => {
            task.id = maxExistingId + index + 1;
            if (!task.createdAt) {
                task.createdAt = new Date().toISOString();
            }
        });

        if (confirm(`템플릿의 할 일 ${validTasks.length}개를 기존 ${tasks.length}개의 할 일과 합치시겠습니까?\n(취소하면 기존 데이터를 대체합니다)`)) {
            // Merge: combine with existing tasks
            tasks = [...tasks, ...validTasks];
        } else {
            // Replace: use imported tasks only
            tasks = validTasks;
        }

        saveTasks();
        renderTasks();
        updateStats();
        
        const templateName = filename.includes('longform') ? '롱폼 있는 날' : '롱폼 없는 날';
        alert(`템플릿 "${templateName}"에서 할 일 ${validTasks.length}개를 성공적으로 불러왔습니다!`);
    } catch (error) {
        console.error('템플릿 로드 오류:', error);
        alert('템플릿 파일을 불러오는 중 오류가 발생했습니다.\n파일이 존재하는지 확인해주세요.');
    }
}

// Event Listeners
addBtn.addEventListener('click', addTask);

taskInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        addTask();
    }
});

filterBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        filterBtns.forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
        currentFilter = btn.dataset.filter;
        renderTasks();
    });
});

clearCompletedBtn.addEventListener('click', () => {
    const completedTasks = tasks.filter(t => t.completed).length;
    if (completedTasks === 0) {
        alert('완료된 할 일이 없습니다.');
        return;
    }
    
    if (confirm(`완료된 ${completedTasks}개의 할 일을 모두 삭제하시겠습니까?`)) {
        tasks = tasks.filter(t => !t.completed);
        saveTasks();
        renderTasks();
        updateStats();
    }
});

clearAllBtn.addEventListener('click', () => {
    if (tasks.length === 0) {
        alert('삭제할 할 일이 없습니다.');
        return;
    }
    
    if (confirm(`모든 할 일(${tasks.length}개)을 삭제하시겠습니까?`)) {
        tasks = [];
        saveTasks();
        renderTasks();
        updateStats();
    }
});

// JSON export/import event listeners
exportJSONBtn.addEventListener('click', exportToJSON);
importJSONInput.addEventListener('change', importFromJSON);

// Template load event listeners
loadDailyBasicBtn.addEventListener('click', () => loadTemplate('daily-basic.json'));
loadDailyLongformBtn.addEventListener('click', () => loadTemplate('daily-longform.json'));

// Initialize app
loadTasks();
updateStats();

// Make functions globally accessible for inline event handlers
window.toggleTask = toggleTask;
window.deleteTask = deleteTask;
