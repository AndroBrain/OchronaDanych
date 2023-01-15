import {useContext, useEffect, useState} from "react";
import ReactMarkdown from 'react-markdown'
import {useNavigate} from "react-router-dom";
import {CreateNoteRequest} from "../request/CreateNoteRequest";
import {authContext} from "../auth/auth";

export function CreateNote() {
    let {authState} = useContext(authContext)
    let [title, setTitle] = useState("")
    let [description, setDescription] = useState("")
    let [error, setError] = useState(null)
    let [isLoading, setIsLoading] = useState(false)
    let [isSuccess, setIsSuccess] = useState(false)
    let navigate = useNavigate()

    const commandCreateNote = (e) => {
        e.preventDefault()
        setError(null)
        setIsLoading(true)
        CreateNoteRequest(setIsLoading, authState.jwt, title, description, setIsSuccess, setError)
    }

    useEffect(() => {
        if (isSuccess)
            navigate("/")
    }, [isSuccess])

    return <div>
        <form onSubmit={commandCreateNote}>
            Note name:
            <br/>
            <input type="text" onChange={e => setTitle(e.target.value)} value={title}/>
            <br/>
            Description:
            <br/>
            <textarea onChange={e => setDescription(e.target.value)} value={description}/>
            <br/>
            Styled input:
            <ReactMarkdown children={description}/>
            <br/>
            <button disabled={isLoading} style={{marginTop: "1em"}} onClick={commandCreateNote}>Create note
            </button>
            {error != null && <p>Error: {error}</p>}
        </form>
    </div>
}
