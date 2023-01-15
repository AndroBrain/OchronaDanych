import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {GetPublicNoteRequest} from "../request/GetPublicNoteRequest";

export function PublicNote() {
    let params = useParams()
    let [note, setNote] = useState(null)
    let [error, setError] = useState(null)

    useEffect(() => {
        GetPublicNoteRequest(params.id, setNote, setError)
    }, [])

    return <div>
        {error != null && <p>Error: {error}</p>}
        {note != null && <div>
            <h3>Name: {note.name}</h3>
            <p>{note.description}</p>
        </div>}

    </div>
}
