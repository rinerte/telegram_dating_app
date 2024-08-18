/* eslint-disable no-unused-vars */
import { useHttp } from "./HttpHook";

const useProfileService = () => {

	const tData = window.Telegram.WebApp.initDataUnsafe;
    let userId = null;
    try{
        userId = tData.user.id;        
    } catch {
        userId = 10;        
    }

    const {loading, getrequest, postrequest, error, clearError} = useHttp();
	const _apiBase = 'https://{DOMAIN}/forms/';

	const postFormData = async (body) => {
		
		const url = `${_apiBase}form?userId=${userId}`;
		const bodyjson = JSON.stringify(body);

		const res = await postrequest(url, bodyjson);
		return res;
	}
	const getForms = async () =>{
		const res = await getrequest(`${_apiBase}forms?userId=${userId}`)
		return res;
	}

	const postDeleteForm = async (formId) => {
		const url = `${_apiBase}delete?userId=${userId}&formId=${formId}`;
		const res = await postrequest(url);
		return res;
	}

	const getForm = async (id) => {
		const res = await getrequest(`${_apiBase}form?userId=${userId}&formId=${id}`)
		
		let newRes = {
			name: res.name,
			questions : []
		}
		let orderCounter = 1;

		res.questionModules.forEach(item => {
			const qModuleId = item.questionModuleId;

			item.questions.forEach(q=> {
				let counter = 0;

				const question = 
				{
					questionId: qModuleId,
					questionTitle: q.title,
					multiselectable: q.multiselectable,
					answers: q.answers,
					orderId: orderCounter++
				}
				counter = counter + 1;
				newRes.questions.push(question);
			});

		});
		console.log(newRes);
		return newRes;
	}

    return {
		loading,
		error,
		clearError,
        getForms,
		getForm,
		postFormData,
		postDeleteForm
    }
}

export default useProfileService;