<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { FwbBadge } from 'flowbite-vue';
import {
  Encounter,
  EncounterAlertType,
  EncounterPriority,
  EncounterStage,
  Job,
  FacilityRoom,
  JobStatus,
  User,
} from '@/api/__generated__/graphql';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useUsersStore } from '@/stores/data/settings/users';
import { IconClockCheck, IconClockFastForward, IconLink04 } from '@/components/icons/index';
import { useLinesStore } from '@/stores/data/encounters/lines';
import EncounterStageBadge from '../badge/EncounterStageBadge.vue';
import { useEncounterTimers } from '@/hooks/useEncounterTimers';
import dayjs from 'dayjs';
import UserAvatar from '@/components/avatar/UserAvatar.vue';

const props = defineProps<{
  encounter: Encounter;
  job: Job;
}>();

const purposesStore = usePurposesStore();
const facilitiesStore = useFacilitiesStore();
const usersStore = useUsersStore();
const linesStore = useLinesStore();

const { relativeTimeEnc, elapsedTime, holdElapsedTime, attendingDuration } = useEncounterTimers(props);

const { encounter } = props;

const onHoldAlert = computed(() => props.encounter.alerts?.filter((alert) => alert?.type === EncounterAlertType.OnHold));
const onHoldAlertText = computed(() => onHoldAlert.value?.[0]?.text);

const cardColor = computed(() => {
  let color;
  switch (encounter.priority) {
    case EncounterPriority.Stat:
      color = { bdr: 'radical-red-700', bg: 'radical-red-50' };
      break;
    case EncounterPriority.Routine:
      color = { bdr: 'blue-700', bg: 'blue-50' };
      break;
    case EncounterPriority.Normal:
      color = { bdr: 'turquoise-blue-500', bg: 'turquoise-blue-50' };
      break;
    default:
      color = { bdr: 'turquoise-blue-500', bg: 'turquoise-blue-50' };
  }
  return color;
});

function getPurposeNameById(id: string): string {
  const purpose = purposesStore.purposes.find((p) => p.id === id);
  return purpose?.name ?? '';
}
function getFacilityNameById(id: string): string {
  const facility = facilitiesStore.facilities.find((f) => f.id === id);
  return facility?.name ?? '';
}

onMounted(() => {
  const facilityId = props.job.location?.facilityId;
  if (facilityId) {
    getRoomsForFacility(facilityId);
  }
});
const roomsCache = ref<FacilityRoom[]>([]);

function getRoomsForFacility(facilityId: string): void {
  roomsCache.value = facilitiesStore.rooms.filter(x=> x.facilityId === facilityId)
}
function getRoomNameById(roomId: string): string {
  const room = roomsCache.value.find((r) => r.id === roomId);
  return room?.name ?? '';
}

function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}

function getLineNameById(lineId: string): string {
  const line = linesStore.lines.find((l) => l.id === lineId);
  return line?.name ?? '(Unspecified)';
}
const assistanceRequest = computed(() =>
  props.encounter?.alerts?.find((x) => x?.type == EncounterAlertType.AssistanceRequested)
);

function formatDate(dateString: string): string {
  return dayjs(dateString).format('ddd, D MMM YYYY');
}
function convertToAmPm(isoDuration: string): string {
  const matches = isoDuration.match(/PT(\d+H)?(\d+M)?/);

  if (!matches) {
    throw new Error('Invalid ISO 8601 duration format');
  }
  const hours = parseInt(matches[1]?.replace('H', '') || '0', 10);
  const minutes = parseInt(matches[2]?.replace('M', '') || '0', 10);
  const period = hours >= 12 ? 'PM' : 'AM';
  const adjustedHours = hours % 12 || 12; // Convert 0 or 24 to 12
  const formattedMinutes = minutes.toString().padStart(2, '0');
  return `${adjustedHours}:${formattedMinutes} ${period}`;
}

function getScheduledDateTime(date: string, time: string): string {
  const formattedTime = convertToAmPm(time);
  const formattedDate = formatDate(date);
  const currentDate = dayjs().format('YYYY-MM-DD');
  if (currentDate === date && time) {
    return `${formattedTime}`;
  }
  if (date && time) {
    return `${formattedDate}, ${formattedTime}`;
  }
  return formattedDate;
}
</script>
<template>
  <div
    class="rounded-lg border-2 cursor-pointer"
    :class="{
      'border-radical-red-700': encounter.priority === EncounterPriority.Stat,
      'border-turquoise-blue-500': encounter.priority === EncounterPriority.Normal,
      'border-blue-700': encounter.priority === EncounterPriority.Routine,
    }"
  >
    <div v-if="onHoldAlert?.length" class="bg-yellow-400 rounded-t-[6px]">
      <div class="flex items-center justify-between pl-4 pr-2 py-1 text-white">
        <div class="text-sm font-bold flex flex-wrap">
          ON HOLD
          <span v-if="onHoldAlertText"> - “{{ onHoldAlertText }}”</span>
        </div>
        <!-- Hold Elasped Time -->
        <fwb-badge
          v-if="props.encounter?.alerts?.some((alert) => alert?.type === EncounterAlertType.OnHold)"
          class="rounded-xl bg-transparent text-white mr-0"
          size="sm"
        >
          <template #icon>
            <IconClockFastForward height="18" width="19" />
          </template>
          <span class="pl-2">{{ holdElapsedTime }}</span>
        </fwb-badge>
      </div>
    </div>

    <div
      v-if="assistanceRequest"
      class="bg-radical-red-400 rounded-t-[6px]"
    >
      <div class="flex items-center justify-between pl-4 pr-2 py-1 text-white">
        <div class="text-sm font-bold flex flex-wrap">
          ASSISTANCE NEEDED
          <span v-if="assistanceRequest?.text && assistanceRequest?.text !== ''"
            > - "{{ assistanceRequest?.text }}"</span>
        </div>
      </div>
    </div>
    <div
      v-if="
        props.job.schedule &&
        props.job.status !== JobStatus.Canceled &&
        props.encounter?.stage !== EncounterStage.Charting &&
        props.encounter?.stage !== EncounterStage.Attending &&
        props.encounter?.stage !== EncounterStage.Completed
      "
      class="bg-turquoise-blue-600 rounded-t-[6px]"
      :class="`border-${cardColor.bdr}`"
    >
      <div class="flex items-center pl-4 pr-2 py-1 text-white text-sm font-bold">
        <span>SCHEDULED FOR:</span>
        <span class="pl-1">{{
          getScheduledDateTime(props.job.schedule.date, props.job.schedule.time)
        }}</span>
      </div>
    </div>
    <div class="p-4 text-slate-500">
      <div class="flex lg:items-center justify-between flex-col lg:flex-row gap-4">
        <div>
          <h3 class="text-base font-semibold text-brand-600">
            {{ getPurposeNameById(job.purposeId) }}
          </h3>
          <p v-if="job.location" class="text-sm font-medium">
            {{ getFacilityNameById(job.location.facilityId) }} &nbsp;-&nbsp;
            {{ getRoomNameById(job.location.roomId) }}
          </p>
        </div>
        <div class="flex items-center gap-3">
          <!-- Attanding Timer -->
          <fwb-badge
            v-if="props.encounter?.stage === EncounterStage.Attending"
            type="dark"
            class="rounded-xl text-slate-900"
            size="sm"
          >
            <template #icon>
              <IconClockFastForward height="18" width="19" />
            </template>
            <span class="pl-2">{{ elapsedTime }}</span>
          </fwb-badge>
          <!-- Attending duration -->
          <fwb-badge
            v-if="props.encounter?.stage === EncounterStage.Charting"
            type="dark"
            class="rounded-xl text-slate-900"
            size="sm"
          >
            <template #icon>
              <IconClockCheck height="18" width="19" />
            </template>
            <span class="pl-2">{{ attendingDuration }}</span>
          </fwb-badge>
          <user-avatar
            v-if="props.encounter.assignments"
            v-for="(assignment, index) in props.encounter.assignments"
            :key="assignment?.clinicianId || index"
            :user="getUserForAvatar(assignment?.clinicianId)"
            rounded
            size="sm"
          />
        </div>
      </div>
      <div v-if="props.encounter.lines?.length" class="pt-4">
        <fwb-badge
          type="dark"
          class="rounded-2xl bg-slate-100 text-slate-900 inline-flex items-center whitespace-nowrap px-2 py-1"
          size="sm"
        >
          <template #icon>
            <IconLink04 height="20" width="20" />
          </template>
          <span class="pl-2">{{ getLineNameById(props.encounter.lines[0]) }}</span>
        </fwb-badge>
      </div>
      <div class="pt-4">
        <h4 v-if="job.contact && job.contact.trim().length !== 0" class="text-sm font-semibold text-slate-900 flex items-center">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            viewBox="0 0 16 16"
            fill="none"
          >
            <g clip-path="url(#clip0_1181_5806)">
              <path
                d="M14.1408 13.2349L11.0269 12.5428C10.5757 12.4425 10.1854 12.0927 9.9824 11.6667H13.4998V9.36892C13.4998 7.64154 12.7993 6.07697 11.6665 4.94432L12.1373 2.38064C12.2198 1.9313 11.9888 1.4782 11.5611 1.31766C9.17526 0.422127 6.68297 0.477105 4.43841 1.318C4.01057 1.47828 3.77969 1.93133 3.86224 2.38069L4.33317 4.94414C3.20054 6.07678 2.49984 7.64136 2.49984 9.36874V10.6665C2.49984 11.2188 2.94755 11.6665 3.49984 11.6665H6.01727C5.81469 12.0925 5.42474 12.4423 4.97355 12.5426L1.85964 13.2347C1.20367 13.3807 0.666504 14.0502 0.666504 14.7221C0.666504 15.0596 0.940169 15.3332 1.27765 15.3332C1.61512 15.3332 1.88879 15.0596 1.88879 14.7221C1.88879 14.6224 2.0272 14.4495 2.1251 14.428L5.23792 13.7358C6.28384 13.5035 7.12974 12.5881 7.33745 11.5438C7.55232 11.6225 7.77379 11.6665 7.99984 11.6665C8.22662 11.6665 8.44735 11.6225 8.66222 11.5436C8.86994 12.5879 9.71675 13.5033 10.7618 13.7356L13.8746 14.4279C13.9725 14.4493 14.1109 14.6224 14.1109 14.7219C14.1109 15.0595 14.3845 15.3332 14.7221 15.3332C15.0595 15.3332 15.3332 15.0596 15.3332 14.7221C15.3332 14.0502 14.796 13.3807 14.1408 13.2349ZM12.2776 9.36892V10.4444H10.0211C10.6972 9.54676 11.2135 8.21355 11.4736 6.64329C11.9954 7.44904 12.2776 8.38643 12.2776 9.36892ZM10.8954 2.3769L10.4241 4.94432H5.57599L5.10427 2.3769C6.99847 1.72332 9.00249 1.72405 10.8954 2.3769ZM3.72212 9.36892C3.72212 8.38662 4.00445 7.44868 4.52604 6.6431C4.78692 8.213 5.303 9.54676 5.97895 10.4444H3.72212V9.36892ZM5.68067 6.16643H10.3188C9.9483 8.8246 8.82154 10.4444 7.99984 10.4444C7.17869 10.4444 6.05174 8.8246 5.68067 6.16643Z"
                fill="#0F172A"
              />
              <path
                d="M9.33317 2.85689H8.47598V2H7.5237V2.85689H6.6665V3.80909H7.5237V4.66667H8.47598V3.80909H9.33317V2.85689Z"
                fill="#0F172A"
              />
            </g>
            <defs>
              <clipPath id="clip0_1181_5806">
                <rect width="16" height="16" fill="white" />
              </clipPath>
            </defs>
          </svg>
          <span class="ml-2">
            {{ job.contact }}
          </span>
        </h4>
        <p v-if="props.job.notes?.[0]?.text" class="text-sm font-medium">
          {{ props.job.notes[0].text }}
        </p>
      </div>
      <div class="pt-4 flex justify-between items-center">
        <p class="text-xs font-medium">{{ relativeTimeEnc }}</p>
        <EncounterStageBadge v-if="props.encounter.stage" :badge="props.encounter.stage" size="xs" />
      </div>
    </div>
  </div>
</template>
