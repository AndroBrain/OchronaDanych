import {apiUrl} from "../App";

export let MakeNotePublicRequest = (jwt, id, setError, setSuccess, setIsLoading) => {
    fetch(`${apiUrl}/Note/PublishNote?id=${id}`,
        {
            "mode": "cors",
            "method": "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                Authorization: `Bearer ${jwt}`
            },
        }
    ).then(response => {
        setIsLoading(false)
        if (response.status === 200) {
            setSuccess("Note made public successfully")
        } else {
            setError(response.status);
        }
    })
        .catch(e => {
            setError(e.toString());
        });
}
