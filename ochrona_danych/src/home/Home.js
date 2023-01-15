import {useContext, useEffect, useState} from "react";
import {authContext} from "../auth/auth";
import {Link, useNavigate} from "react-router-dom";
import {GetPublicNotesRequest} from "../request/GetPublicNotesRequest";
import {GetPrivateNotesRequest} from "../request/GetPrivateNotesRequest";

export function Home() {
    let {authState} = useContext(authContext)
    let [privateNotes, setPrivateNotes] = useState([])
    let [privateNotesError, setPrivateNotesError] = useState(null)

    let [publicNotes, setPublicNotes] = useState([])
    let [publicNotesError, setPublicNotesError] = useState(null)
    const navigate = useNavigate()

    useEffect(() => {
        setPrivateNotesError(null)
        GetPrivateNotesRequest(authState.jwt, setPrivateNotes, setPrivateNotesError)
    }, [authState])

    useEffect(() => {
        setPublicNotesError(null)
        GetPublicNotesRequest(setPublicNotes, setPublicNotesError)
    }, [])

    return (
        <div>
            HOME
            {authState.jwt != null && <div>
                <h2>Your notes</h2>
                <button onClick={(e) => navigate("/create-note")}>Create new Note</button>
                {privateNotesError != null && <p>Error: {publicNotesError}</p>}
                <ul>
                    {privateNotes.map(note => (
                        <li key={note.id}>
                            <Link to={`private/${note.id}`}>Name: {note.name}</Link>
                        </li>
                    ))}
                </ul>
            </div>}

            <div style={{marginTop: "16px"}}>
                {authState.jwt === null && <button onClick={(e) => navigate("/login")}>Login</button>}
            </div>
            <div style={{marginTop: "16px"}}>
                {authState.jwt === null && <button onClick={(e) => navigate("/register")}>Register</button>}
            </div>
            <h2>Public notes</h2>
            {publicNotesError != null && <p>Error: {publicNotesError}</p>}
            <ul>
                {publicNotes.map(note => (
                    <li key={note.id}>
                        <Link to={`public/${note.id}`}>Name: {note.name}</Link>
                    </li>
                ))}
            </ul>
        </div>
    )
}
