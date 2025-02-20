<script setup lang="ts">
import { Routine } from '@/api/__generated__/graphql';
import { IconPlusCircle, IconClock, IconCalendar } from '@/components/icons/index';
import { reactive, ref, computed, watch, onMounted } from 'vue';

const props = defineProps<{
  routine?: Routine;
}>();
const emit = defineEmits(['complete-setp', 'recurring-updated']);


onMounted(() => {
  if (props.routine?.recurrence?.length) {
    updateEvents(props.routine.recurrence); // Initial assignment on mount
  }
});

function updateEvents(newRecurrence: any[]) {
  const filteredRecurrence = newRecurrence.map(({ __typename, ...rest }) => ({ ...rest }));
  events.splice(0, events.length, ...JSON.parse(JSON.stringify(filteredRecurrence)));
  console.log('events', events);
}

// Recurrence events
const daysOfWeek = ref([
  { name: 'Sun', isoDay: 7 },
  { name: 'Mon', isoDay: 1 },
  { name: 'Tue', isoDay: 2 },
  { name: 'Wed', isoDay: 3 },
  { name: 'Thu', isoDay: 4 },
  { name: 'Fri', isoDay: 5 },
  { name: 'Sat', isoDay: 6 },
]);

const events = reactive<any[]>([
  {
    time: '',
    days: [] as number[],
  },
]);

// Map event days to day names
const getDayNames = (days: number[]) => {
  return days.map((day) => daysOfWeek.value.find((d) => d.isoDay === day)?.name || '');
};

// Toggle day selection for each event, storing ISO day of week
const toggleDay = (eventIndex: number, day: { name: string; isoDay: number }) => {
  const days = events[eventIndex].days;
  if (days.includes(day.isoDay)) {
    events[eventIndex].days = days.filter((d: number) => d !== day.isoDay);
  } else {
    events[eventIndex].days.push(day.isoDay);
  }
};

//  Add event
const addEvent = () => {
  events.push({
    time: '',
    days: [] as number[],
  });
};
const isRecurrenceComplete = computed(() => {
  return !!events.every((event) => event.time && event.days.length > 0);
});

watch(isRecurrenceComplete, (newValue) => {
  emit('complete-setp', newValue);
});

watch(events, () => {
  emit('recurring-updated', events);
});
</script>

<template>
  <div>
    <div v-for="(event, index) in events" :key="index" class="mb-8">
      <div class="flex gap-3">
        <span
          class="p-1 w-[40px] h-[40px] flex items-center justify-center text-xs leading-[18px] font-semibold rounded bg-slate-100"
        >
          {{ index + 1 }}
        </span>
        <div class="flex-1">
          <div class="flex-1 flex items-center">
            <div class="sm:w-24 sm:px-0 px-2 text-sm font-medium text-[#000]">At</div>
            <div class="relative flex-1">
              <div
                class="absolute inset-y-0 end-0 top-0 flex items-center pe-3.5 pointer-events-none"
              >
                <IconClock />
              </div>
              <input
                v-model="event.time"
                type="time"
                id="time"
                step="1"
                class="bg-gray-50 border leading-none border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                required
              />
            </div>
          </div>

          <div class="flex-1 flex items-center mt-4">
            <div class="sm:w-24 sm:px-0 px-2 text-sm font-medium text-[#000]">On</div>
            <div class="flex-1">
              <div class="mb-4 flex flex-wrap gap-2">
                <button
                  v-for="day in daysOfWeek"
                  :key="day.isoDay"
                  type="button"
                  @click="toggleDay(index, day)"
                  :class="[
                    'flex justify-center items-center capitalize py-2 px-2 min-w-10 text-xs font-semibold',
                    event.days.includes(day.isoDay)
                      ? 'bg-brand-50 text-brand-600 border border-brand-600 rounded-md'
                      : 'bg-white text-slate-500',
                  ]"
                >
                  {{ day.name }}
                </button>
              </div>
              <div
                class="py-2 px-4 rounded-lg text-sm font-semibold capitalize text-slate-800 flex-1 flex items-center justify-between bg-brand-50"
              >
                <span v-if="event.days.length > 0">
                  Every {{ getDayNames(event.days).join(', ') }}
                </span>
                <span v-else>No days selected</span>
                <IconCalendar />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <button @click="addEvent" class="flex gap-2 items-center text-brand-600 text-sm font-medium">
      <IconPlusCircle /> Add Event
    </button>
  </div>
</template>
