import {useState, useCallback} from 'react';

export const useHttp = () =>{
    const [loading,setLoading] = useState(false);
    const [error,setError] = useState(null);

    const getrequest = useCallback(async (url,method = 'GET', body = null, headers = {'Content-Type':'application/json'}) => {

        setLoading(true);

        try{
            const response = await fetch(url,{method,body,headers});

            if (!response.ok) {
                throw new Error(`Could not fetch ${url}, status: ${response.status}`);
            }

            const data = await response.json();
            
            setLoading(false);
            return data;
        } catch(e){
            
            setLoading(false);
            setError(e.message);
            throw e;
        }
    },[]);

    const postrequest =  useCallback(async (url, body = null, method = 'POST', headers = {'Accept': 'application/json','Content-Type':'application/json'}) =>{
        setLoading(true);

        try{
            const response = await fetch(url,{method,body,headers});

            
            if (!response.ok) {
                throw new Error(`Could not fetch ${url}, status: ${response.status}`);
            }
            
            setLoading(false);
            return 'data sent';
        } catch(e){            
            setLoading(false);
            setError(e.message);
            throw e;
        }

    },[]);



    const clearError = useCallback(()=> setError(null),[]);

    return {loading, getrequest,postrequest, error, clearError}
}