import {useParams} from "react-router-dom";

import {useContext, useEffect, useState} from "react";
import {authContext} from "../auth/auth";
import {GetPrivateNoteRequest} from "../request/GetPrivateNoteRequest";
import ReactMarkdown from "react-markdown";
import {MakeNotePublicRequest} from "../request/MakeNotePublicRequest";
import {DecryptNoteRequest} from "../request/DecryptNoteRequest";
import {EncryptNoteRequest} from "../request/EncryptNoteRequest";
import {CheckIsPasswordValid, CheckPasswordStrength} from "../password/PasswordCheck";

export function PrivateNote() {
    let {authState} = useContext(authContext)
    let params = useParams()
    let [decryptNotePassword, setDecryptNotePassword] = useState("")
    let [encryptNotePassword, setEncryptNotePassword] = useState("")
    let [passwordStrength, setPasswordStrength] = useState("weak")
    let [note, setNote] = useState(null)
    let [isLoading, setIsLoading] = useState(false)
    let [error, setError] = useState(null)
    let [success, setSuccess] = useState(null)

    useEffect(() => {
        if (authState.jwt != null)
            GetPrivateNoteRequest(authState.jwt, params.id, setNote, setError)
    }, [authState])

    const commandDecryptNote = (e) => {
        e.preventDefault()
        setIsLoading(true)
        DecryptNoteRequest(authState.jwt, note.id, decryptNotePassword, note, setNote, setError, setIsLoading)
    }

    const commandMakePublic = (e) => {
        e.preventDefault()
        setIsLoading(true)
        MakeNotePublicRequest(authState.jwt, note.id, setError, setSuccess, setIsLoading)
    }

    const commandEncryptNote = (e) => {
        e.preventDefault()
        if (isLoading)
            return
        setIsLoading(true)
        if (CheckIsPasswordValid(encryptNotePassword)) {
            EncryptNoteRequest(authState.jwt, note.id, encryptNotePassword, setError, setSuccess, setIsLoading)
        }
    }

    return <div>
        {error != null && <p>Error: {error}</p>}
        {note != null && <div>
            <h3>Name: {note.name}</h3>
            Description:
            {note.description !== null && <ReactMarkdown children={note.description}/>}
            {(note.isEncrypted === true && note.description === null) && <div>
                <h4>Note is encrypted. Enter password to read it.</h4>
                <p>Password:</p>
                <form onSubmit={commandDecryptNote}>
                    <input type="password" onChange={e => setDecryptNotePassword(e.target.value)}
                           value={decryptNotePassword}/>
                    <br/>
                    <button onClick={commandDecryptNote}>Decrypt note</button>
                </form>
            </div>
            }
            {(note.isPublic !== true && note.isEncrypted !== true && success === null) && <div>
                <div>
                    <button disabled={isLoading} onClick={commandMakePublic}>Make public</button>
                </div>
                <div style={{marginTop: "8px"}}>
                    <form onSubmit={commandEncryptNote}>
                        <input type="password" onChange={e => {
                            setEncryptNotePassword(e.target.value)
                            setPasswordStrength(CheckPasswordStrength(e.target.value))
                        }}
                               value={encryptNotePassword}/>
                        <br/>
                        <span>Password strength: {passwordStrength}</span>
                        <br/>
                        <span>Password must have at least 1 lowercase character, 1 uppercase character, 1 number and 1 special character(!@#$%^&*)</span>
                        <br/>
                        <button disabled={isLoading} onClick={commandEncryptNote}>Encrypt</button>
                    </form>
                </div>
            </div>
            }
        </div>}
    </div>
}
