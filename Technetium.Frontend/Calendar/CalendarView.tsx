import {Calendar, momentLocalizer} from 'react-big-calendar';
import moment from 'moment';
import React from "react";
import {CalendarViewModel} from "./CalendarViewModel.js";

const localizer = momentLocalizer(moment);

export const CalendarView = ({viewModel}: { viewModel: CalendarViewModel }) => (
    <Calendar
        localizer={localizer}
        events={[/* TODO[#23]: Pass the events here */]}
        selectable={true}
        onSelecting={viewModel.onSelecting.bind(viewModel)}
        style={{height: 500}}
    />
);
