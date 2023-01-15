import {apiUrl} from "../App";

export let RegisterRequest = (setIsLoading,setSuccess, setError, email, name, password) => {
    fetch(`${apiUrl}/Authentication/Register`,
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
                    name: name,
                    password: password
                }
            )

        }
    )
        .then(response => {
            setIsLoading(false)
            if (response.status === 200) {
                setSuccess(true)
            } else {
                try {
                    response.json().then(json => {
                        setError(json)
                        // setError(response.status + " " + json.title)
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
