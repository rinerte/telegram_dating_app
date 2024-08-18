/* eslint-disable no-unused-vars */
import {useState, useEffect, useMemo} from 'react';
import Button from './Button';
import LoadingSpinner from './Spinner';
import useProfileService from '../services/ProfileService';
import refreshIcon from '../assets/free-icon-refresh.png';
import deleteIcon from '../assets/free-icon-trash.png';
import playIcon from '../assets/free-icon-play.png';

function Stats({selectForm}) {
    const [forms, setForms] = useState([]);

    useEffect(()=>{
        updateForms();
    },[]);

    const {loading,error,getForms,postDeleteForm, clearError} = useProfileService();

    const updateForms = () => {
        clearError();
        getForms().then(setForms);        
    }
    const onStartForm = (id) => {        
        selectForm(id);
    }

    const onResetForm = async (id) => {        
        await postDeleteForm(id);
        updateForms();
    }

    const onStartOverForm = async (id) => {
        clearError();
        await postDeleteForm(id);      
        selectForm(id);
    }

    function completeList(completeForms){
        if(completeForms.length!=0) {
            return (
                <>
                <h2 className='
                mt-[100px] mb-[20px] mx-[15px]
                flex justify-center
                text-primaryWhite font-russoOne font-[400] text-[15px] tracking-wide 
                '>Я приступил к поиску и напишу вам, как только найду подходящего человека <br/> (не выключайте уведомления!) <br/><br/>А пока можете закрыть это окно или пройти другую анкету</h2>
                <h2 className='
                mt-[20px] mb-[40px]
                flex justify-center
                text-primaryWhite font-russoOne font-[400] text-[20px] tracking-wide
                '> Ведётся поиск: </h2>
                {listComplete(completeForms)}
                </>
            )
        } else return null;
    }
    function uncompleteList(incompleteForms){
        if(incompleteForms.length!=0) {
            return (
                <> 
                <h2 className='
                mt-[100px] mb-[40px]
                flex justify-center
                text-primaryWhite font-russoOne font-[400] text-[20px] tracking-wide
                '> Доступные анкеты</h2>
                {listUnComplete(incompleteForms)}
                </>
            )
        } else return null;
    }
    function renderForms(arr){
        if(error) return (<> <h1>ERRROOOOOORR..</h1></>);
        if(loading) return (<> <LoadingSpinner/> </>);

        const completeForms = arr.filter(item=>item.percentage==100);
        const incompleteForms = arr.filter(item=>item.percentage!==100);

        const listOne = completeList(completeForms);
        const listTwo = uncompleteList(incompleteForms);
        const listNone = () => {
            if(listOne==null && listTwo==null) {
                return (
                    <h2 className='
                    mt-[100px] mb-[40px]
                    flex justify-center
                    text-primaryWhite font-russoOne font-[400] text-[20px] tracking-wide
                    '> Нет доступных анкет</h2>
                )
            } else {
                return null;
            }
        }
        return (
            <>
                {listOne}
                {listTwo}
                {listNone()}
            </>
        )
    }

    function listUnComplete(arr){
        const btnStyle = ' h-[100%] min-w-[75px] rounded-[0px] px-0 py-0 mx-[0px]';
        const refIcon = <img src={refreshIcon} alt="refresh" className='w-[20px] h-[20px] mx-auto invert'/>
        const plIcon = <img src={playIcon} alt="start" className='w-[20px] h-[20px] mx-auto invert'/>

        const items = arr.sort((a,b)=>(a.percentage-b.percentage)).map((item)=>{
            return (
                <li className='
                h-[50px] w-[95%] flex items-center justify-end
                my-1 mx-auto px-4 pr-0
                bg-gradient-to-t from-[rgb(60,55,127)] to-[rgb(123,113,252)]
                rounded-[20px] border-[2px] border-purpleBorder
                '
                tabIndex={0}
                key={item.id}
                >
                    
                    <h3 className='
                    w-full px-4
                    text-primaryWhite font-russoOne font-[400] text-[16px] tracking-wide
                    '>{item.name} </h3>
                    {item.percentage>0 ? <Button Tip='Start Over' Caption={refIcon} Style={btnStyle} OnClick={()=>onStartOverForm(item.id)}/> : ''}
                    <Button Tip='Start' Caption={plIcon} Style={btnStyle + ' rounded-r-[18px]' } OnClick={()=>onStartForm(item.id)}/>
                </li>
            )
        });
        return (
            <ul className="char__grid mb-[150px]">
                {items}
            </ul>
        )
    }
    function listComplete(arr){
        const btnStyle = 'h-[100%] min-w-[75px] rounded-[0px] px-0 py-0 mx-[0px]'
        const items = arr.map((item)=>{
        const refIcon = <img src={refreshIcon} alt="refresh" className='w-[20px] h-[20px] mx-auto invert'/>
        const delIcon = <img src={deleteIcon} alt="delete" className='w-[20px] h-[20px] mx-auto invert'/>

            return (
                <li className='
                h-[50px] w-[95%] flex items-center justify-end
                my-1 mx-auto px-4 pr-0
                bg-gradient-to-t from-[rgb(60,55,127)] to-[rgb(123,113,252)]
                rounded-[20px] border-[2px] border-purpleBorder
                '
                tabIndex={0}
                key={item.id}
                >
                    
                    <h3 className='
                    w-full px-4
                    text-primaryWhite font-russoOne font-[400] text-[16px] tracking-wide
                    '>{item.name} </h3>
                    <Button Tip='Start Over' Caption={refIcon} Style={btnStyle} OnClick={()=>onStartOverForm(item.id)}/>
                    <Button Tip='Reset form' Caption={delIcon} Style={btnStyle + ' rounded-r-[18px]'} OnClick={()=>onResetForm(item.id)}/>
                </li>
            )
        });
        return (
            <ul className="char__grid">
                {items}
            </ul>
        )
    }

    

    const elements = useMemo(() => {
        return renderForms(forms, error, loading);
        
    }, [forms, error, loading])
  return (
    <>
        {elements}
    </>
  )
}

export default Stats