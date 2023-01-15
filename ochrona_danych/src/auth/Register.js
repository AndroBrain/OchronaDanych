import {useState} from "react";
import {Navigate, useNavigate} from "react-router-dom";
import {RegisterRequest} from "../request/RegisterRequest";
import {CheckIsPasswordValid, CheckPasswordStrength} from "../password/PasswordCheck";

export function Register() {
    const [email, setEmail] = useState("")
    const [name, setName] = useState("")
    const [password, setPassword] = useState("")
    const [passwordStrength, setPasswordStrength] = useState("weak")
    const [isSuccess, setIsSuccess] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState(null)

    const navigate = useNavigate()

    const commandRegister = (e) => {
        e.preventDefault()
        if (isLoading)
            return
        if (CheckIsPasswordValid(password)) {
            setIsLoading(true)
            RegisterRequest(setIsLoading, setIsSuccess, setError, email, name, password)
        }
    }

    return <div>
        {isSuccess && <Navigate to="/login"/>}
        {error ?
            <div style={{marginBottom: "1em"}}>{error}</div> : ""
        }
        <form onSubmit={commandRegister}>
            Email:
            <br/>
            <input type="email" onChange={e => setEmail(e.target.value)} value={email}/>
            <br/>
            Name:
            <br/>
            <input type="text" onChange={e => setName(e.target.value)} value={name}/>
            <br/>
            Password:
            <br/>
            <input type="password" onChange={e => {
                setPassword(e.target.value)
                setPasswordStrength(CheckPasswordStrength(e.target.value))
            }} value={password}/>
            <br/>
            <span>Password strength: {passwordStrength}</span>
            <br/>
            <span>Password must have at least 1 lowercase character, 1 uppercase character, 1 number and 1 special character(!@#$%^&*)</span>
            <br/>
            <button disabled={isLoading} style={{marginTop: "1em"}} onClick={commandRegister}>Register
            </button>
            <button style={{marginTop: "1em"}} onClick={(e) => navigate("/login")}>Go to login page
            </button>
        </form>
    </div>
}
