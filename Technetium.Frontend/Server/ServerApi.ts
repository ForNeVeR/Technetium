export async function importTasks(file: File) {
    const fileContent = await file.text();
    const response = await fetch('/api/task/import/google', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: fileContent,
    });

    if (response.status != 200) {
        throw `Error ${response.status}: ${response.statusText}`;
    }
}
