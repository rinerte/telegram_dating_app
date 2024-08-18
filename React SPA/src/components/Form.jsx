import {useState, useEffect, useCallback, useMemo} from 'react';
import useProfileService from '../services/ProfileService';
import botMulti from '../assets/profile_4.png';
import botSingle from '../assets/profile_2.png';

import Button from './Button';
import LoadingSpinner from './Spinner';

function Form({formId, onEnd}) {
    const [form, setForm] = useState(null);
    const [questionNumber, setQuestionNumber] = useState(0);
    const [question, setQuestion] = useState(null);
    const [answers, setAnswers] = useState({});
    const [done, setDone] = useState(false);

    const {loading,error,getForm, clearError,postFormData} = useProfileService();


    const updateForm = () =>{
        clearError();
        getForm(formId).then(setForm);
    }

    useEffect(()=>{
        updateForm();
    },[]);

    useEffect(()=>{
        setQuestionNumber(1);
        
    },[form]);

    useEffect(()=>{
        if(form!=null)
        setQuestion(form.questions.find(item => item.orderId==questionNumber))
    },[questionNumber, form]);
    
    useEffect(() => {
        if(done){
            sendData();
        }
        
    }, [done]);
    
    const setAnswerForMultiselectable = (questionId, answerId) => {
        setAnswers(answers => {
            
            let newAnswers = structuredClone(answers);
            newAnswers[questionId] = newAnswers[questionId] ? newAnswers[questionId] : new Object();

            newAnswers[questionId][answerId] =  newAnswers[questionId][answerId] ? false : true;

            return newAnswers;
        })
    }
    
    const setAnswerForSingle = (questionId, answerId) => {
        setAnswers(answers => {
            
            let newAnswers = structuredClone(answers);
            newAnswers[questionId] =  new Object();
            newAnswers[questionId][answerId] = true;
            return newAnswers;
        });
        const lastQuestion = (form.questions.length==questionNumber)
        lastQuestion ? setDone(true) : nextQuestion();
        
    }

    const nextQuestion = () => {
        setQuestionNumber(questionNumber => questionNumber+1);
    }
    const prevQuestion = () => {
        setQuestionNumber(questionNumber => questionNumber-1);
    }

    const sendData = async () =>{
        let formDataToSend = {
            formName: form.name,
            formId: formId,
            questions: []
        };

        for(let i=0; i<form.questions.length; i++) {
            let answersId = [];
            for(const [key,value] of Object.entries(answers[i+1])){
                if(answers[i+1][key]) answersId.push(key)
            }
            formDataToSend.questions.push({
                moduleId: form.questions[i].questionId,
                title: form.questions[i].questionTitle,
                answers: answersId
            });
        }

        await postFormData(formDataToSend);
        onEnd();
    }
    

    const renderContent = (question) => {

        if(question!=null){
            
            const lastQuestion = (form.questions.length==questionNumber);
            const firstQuestion = (questionNumber==1);
            
            const somethingSelected = () => {
                if(answers[questionNumber] === undefined) return false;
                console.log(answers[questionNumber]);
                
                for(let key in answers[questionNumber]){
                        if(answers[questionNumber][key]==true) return true;
                    }
                return false;
            }

            const image = question.multiselectable? (
                <div className='
                inline-flex m-0 p-0 justify-center translate-y-[26px] ml-[15%]
                '>
                <img src={botMulti} alt="bot image" className='
                w-[130px] h-[82px]
                 rotate-[5deg]
                
                '/>
                <div className='
                flex text-center items-center
                px-[12px] ml-[5px] mt-[12px]
                min-h-[90px] w-[250px]
                bg-white text-black text-[12px] font-prostoOne
                -translate-y-[70px] -translate-x-2
                border border-primaryBlack rounded-[50px] rounded-bl-none box-border
                '>
                   {lastQuestion? `Выберите несколько вариантов и нажмите кнопку "Завершить анкету"` : `Выберите несколько вариантов и нажмите кнопку "Следующий вопрос"`} 
                </div>
                </div>
            ) :
            (
                <img src={botSingle} alt="bot image" className='
                w-[124px] h-[115px]
                translate-y-[35px]
                ml-[10%]
                '/>
            );
            const buttons = question.multiselectable? (
                <>
                <div className='
                flex flex-wrap justify-around
                px-[10%]
                '>
                    {question.answers.map((item)=>{
                        const selected  = answers[questionNumber]? answers[questionNumber][item.answerId] ? true : false : false;
                        return (
                            <Button Caption={item.caption} key={item.answerId} Animate={true}
                            Style='min-h-[50px] rounded-[22px] min-w-[40%] mb-[15px] px-[15px] py-[5px]'
                            Selected={selected}
                            OnClick={()=>{setAnswerForMultiselectable(questionNumber,item.answerId)}}
                        />
                        )
                        
                    })}
                </div>
                <div className='flex w-full'>
                {lastQuestion ? 
                somethingSelected() ?
                <Button Caption={"Завершить анкету"} Style={`min-w-[80%] mx-auto rounded-[22px]  py-5 px-9 my-[12px] mx-[12px]`}
                 OnClick={()=>setDone(true)}
                 />
                 :
                 <Button Caption={"Завершить анкету"} Style={`min-w-[80%] mx-auto rounded-[22px]  py-5 px-9 my-[12px] mx-[12px]`}
                 OnClick={()=>setDone(true)} Disabled={true}
                 />
                :
                somethingSelected() ?
                <Button Caption={"Следующий вопрос"} Style={`min-w-[80%] mx-auto rounded-[22px]  py-5 px-9 my-[12px] mx-[12px]`}
                 OnClick={()=>nextQuestion()}/>
                 :
                 <Button Caption={"Следующий вопрос"} Style={`min-w-[80%] mx-auto rounded-[22px]  py-5 px-9 my-[12px] mx-[12px]`}
                 OnClick={()=>nextQuestion()} Disabled={true}/>
                }
                </div>
                <div className='w-full'>
                    {firstQuestion ? '' :
                    <Button Caption={"<<"} Style={`min-w-[50px] ml-[10%] rounded-[100%] py-3 px-3 my-[40px] mx-[12px]`} OnClick={()=>prevQuestion()}/>
                    }
                </div>
                </>
            ) :
            (
                <>
                <div className='
                flex flex-wrap justify-around
                px-[10%] 
                '>
                    {question.answers.map((item)=>{
                        const selected  = answers[questionNumber]? answers[questionNumber][item.answerId] ? true : false : false;
                        return (
                            <Button Caption={item.caption} key={item.answerId} Animate={true}
                            Style='min-h-[50px] rounded-[22px] min-w-[40%] mb-[15px] px-[15px] py-[5px]'
                            Selected={selected}
                            OnClick={()=>{setAnswerForSingle(questionNumber,item.answerId)}}
                        />
                        )
                        
                    })}
                </div>
                
                <div className='w-full'>
                    {firstQuestion ? '' :
                    <Button Caption={"<<"} Style={`min-w-[50px] ml-[10%] rounded-[100%] py-3 px-3 my-[40px] mx-[12px]`} OnClick={()=>prevQuestion()}/>
                    }
                </div>
                </>
            )
            return(
            <div>
                <div className='mt-[100px]'>
                    {image}
                </div>
                <div className='
                flex items-center justify-center
                bg-white
                mx-auto mb-[25px] mt-[0px] p-[15px] max-w-[80%] min-h-[100px]
                shadow-lg rounded-[30px]
                font-russoOne text-[18px]
                '>
                    {question.questionTitle}
                </div>

                    {buttons}
                
            </div>
            )
        }
    }

    

    const elements = useMemo(() => {
        if(error) return (<> <h1>ERRROOOOOORR..</h1></>);
        if (loading) return (<> <LoadingSpinner/></>);
        
        return renderContent(question);
        
    }, [error, loading ,form, question, questionNumber, answers]);

  return (
    <>
    {elements}
    </>
  )
}

export default Form