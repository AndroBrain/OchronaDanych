import {apiUrl} from "../App";

export let LoginRequest = (setIsLoading,setAuthState, setError, email, password) => {
    fetch(`${apiUrl}/Authentication/Login`,
        {
            "mode": "cors",
            "method": "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            "body": JSON.stringify(
                {
                    email: email,
                    password: password
                }
            )

        }
    )
        .then(response => {
            setIsLoading(false)
            if (response.status === 200) {
                response.json().then(json => {
                    setAuthState({
                        jwt: json
                    })
                });
            } else {
                try {
                    response.json().then(json => {
                        setError(json)
                    })
                } catch {
                    setError(response.status);
                }
            }
        })
        .catch(e => {
            setError(e.toString());
        });
}
