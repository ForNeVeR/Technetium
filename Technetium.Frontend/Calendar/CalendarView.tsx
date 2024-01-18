import {Calendar, momentLocalizer} from 'react-big-calendar';
import moment from 'moment';
import React from "react";

const localizer = momentLocalizer(moment);

export const CalendarView = () => (
    <div>
        <Calendar
            localizer={localizer}
            events={[/* TODO[#23]: Pass the events here */]}
            startAccessor="start"
            endAccessor="end"
            style={{ height: 500 }}
        />
    </div>
);
