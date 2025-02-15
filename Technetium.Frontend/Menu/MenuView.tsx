import React, {useState} from "react";
import {importTasks} from "../Server/ServerApi.js";
import {noAwait} from "../Util/Promises.js";

function processImport(event: React.MouseEvent, file: File) {
    event.preventDefault();

    console.log('Import file start…');
    noAwait(importTasks(file));
}

export const MenuView = () => {
    const [selectedFile, setSelectedFile] = useState<File>();

    return (
        <div>
            <b>Menu</b>
            <form>
                <input type="file"
                       onChange={e => setSelectedFile(e.target.files?.[0])}/>
                <button onClick={event => processImport(event, selectedFile!)}
                        disabled={selectedFile == undefined}>Import Tasks
                </button>
            </form>
        </div>
    )
};
