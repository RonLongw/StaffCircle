<template>
  <v-container class="sms-composer">
    <v-layout row wrap>
      <v-flex xs12>
        <v-form v-model="valid">
          <v-text-field v-model="number" label="Mobile Number"></v-text-field>
          <v-textarea v-model="message" label="Sms Content"></v-textarea>
          <v-btn v-on:click="SendSms()" class="pink white--text">
            <v-icon left>sms</v-icon>
            <span>Send Sms</span>
          </v-btn>
        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import axios from 'axios'

export default {
  name: 'SmsComposer',
  data: () => ({
    valid: true,
    number: '',
    message: '',
    loading: false
  }),
  methods: {
    SendSms () {
      this.loading = true
      axios.post('https://localhost:44391/api/sms', {
        number: this.number,
        message: this.message.substring(0, 159)
      }).then((response) => {
        this.loading = false
        alert(response)
      }).catch((error) => {
        alert(error)
      })
      alert('number: ' + this.number + ', message:' + this.message.substring(0, 159))
    }
  }
}

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="stylus">
</style>
