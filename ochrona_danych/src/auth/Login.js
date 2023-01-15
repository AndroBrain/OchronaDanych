import {useContext, useState} from "react";
import {authContext} from "./auth";
import {LoginRequest} from "../request/LoginRequest";
import {Navigate, useNavigate} from "react-router-dom";

export function Login() {
    const [email, setEmail] = useState("")
    const [isLoading, setIsLoading] = useState(false)
    const [password, setPassword] = useState("")
    const [error, setError] = useState(null)
    const navigate = useNavigate()

    let {authState, setAuthState} = useContext(authContext)
    const commandLogin = (e) => {
        e.preventDefault()
        if (isLoading)
            return
        setError(null)
        setIsLoading(true)
        LoginRequest(setIsLoading, setAuthState, setError, email, password)
    }
    return <div>
        {authState.jwt != null && <Navigate to="/"/>}
        {error ?
            <div style={{marginBottom: "1em"}}>{error}</div> : ""
        }
        <form onSubmit={commandLogin}>
            Email:
            <br/>
            <input type="email" onChange={e => setEmail(e.target.value)} value={email}/>
            <br/>
            Password:
            <br/>
            <input type="password" onChange={e => setPassword(e.target.value)} value={password}/>
            <br/>
            <button disabled={isLoading} style={{marginTop: "1em"}} onClick={commandLogin}>Login
            </button>
            <button style={{marginTop: "1em"}} onClick={(e) => navigate("/register")}>Go to register page
            </button>
        </form>
    </div>
}
