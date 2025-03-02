<script setup lang="ts">
import { useModalStore } from '@/stores/modal';
import { useUsersStore } from '@/stores/data/settings/users';
import { Activity, User } from '@/api/__generated__/graphql';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import DateTimeFormatter from '@/utils/dateTimeFormatter';

const props = defineProps<{
  activity: Activity
}>();

const modalStore = useModalStore();
const usersStore = useUsersStore();

const getUsername = (id: string) => {
  const user = usersStore.users.find((x) => x.id === id);
  if(user){
    return user.firstName + ' ' + user.lastName;
  }
  return '';
};
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}
</script>

<template>
  <div class="border-b border-slate-300 flex items-start md:gap-4 gap-2 py-4">
    <!-- Avatar -->
    <div>
      <user-avatar :user="getUserForAvatar(props.activity.userId)" rounded size="md"/>
    </div>
    <!-- Content -->
    <div class="flex flex-col w-full">
      <span class="text-xs text-slate-500 font-medium">{{ DateTimeFormatter.formatDatetime(props.activity.timestamp) }}</span>
      <div class="text-sm text-slate-800 font-medium flex flex-wrap gap-1">
        <span> {{getUsername(props.activity.userId)}}</span>
        <slot name="action"></slot>
      </div>
      <!-- Details Data -->
      <div :class="['mt-4', modalStore.isLineSidebarOpen ? ' md:w-[520px]' : 'max-w-3xl']">
        <slot name="details"></slot>
      </div>
    </div>
  </div>
</template>
